using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private float moveSpeed = 0.1f;
    public Transform imgs;
    public Transform textPos;
    public TextMeshPro text;
    public TextMeshPro textOuter;
    public GameObject targetObj;
    public GameObject HeartUI;

    //_type: coin이나 cherry UI오브젝트 네임
    //type의 재화를 val의 금액만큼 더함
    public void AddMoney(string _type, int _val)
    {
        gameObject.SetActive(true);

        //금액 텍스트 넣고 올라가는 효과
        text.text = "+" + string.Format("{0:#,0}", _val);
        textOuter.text = "+" + string.Format("{0:#,0}", _val);
        TextUp();

        //코인 이미지가 UI이미지를 향해 움직이는 효과
        CoinUp(_type);

        //금액증가
        State.instance.PlusMoney(_val);
    }

    //텍스트 위치 조금 위로 설정해서 모션시작
    public void TextUp()
    {
        Vector3 toPos = textPos.transform.position;
        toPos.y = text.transform.position.y + 0.5f;
        //text.transform.Find("TextOutline").gameObject.GetComponent<TextmeshOutline>().SetOutline(); 프로로 바꿔서 아웃라인기능대체
        StartCoroutine(MoveTo(textPos, toPos));
    }

    public void CoinUp(string _type)
    {
        if (_type == "coin")
            targetObj = GameObject.Find("Canvas").transform.Find("Top").transform.Find("Money").transform.Find("Image").gameObject;
        else if (_type == "cherry")
            targetObj = GameObject.Find("Canvas").transform.Find("Top").transform.Find("Cherry").transform.Find("Image").gameObject;
        else Debug.Log("화폐오류");

        Vector3 toPos = PosSet();
        
        StartCoroutine(MoveImgChild(toPos));
    }

    public Vector3 PosSet()
    {
        //게임UI 화폐를 obj에 넣고 위치 추출
        Canvas canvas = GameObject.Find("Canvas").gameObject.GetComponent<Canvas>();
        RectTransform targetRect = targetObj.GetComponent<RectTransform>();
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, targetRect.position);
        Vector3 result = Vector3.zero;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(targetRect, screenPos, canvas.worldCamera, out result);
        return result;
    }

    //코인 하나씩 코루틴 호출
    IEnumerator MoveImgChild(Vector3 moveToPos)
    {
        for (int i = 0; i < imgs.childCount; i++)
        {
            StartCoroutine(MoveTo(imgs.GetChild(i).transform, moveToPos));
            yield return new WaitForSeconds(0.1f);
        }
    } 
    IEnumerator MoveTo(Transform _transform, Vector3 moveToPos)
    {
        _transform.gameObject.SetActive(true);

        Vector3 originPos = _transform.position;
        Vector3 originScale = _transform.localScale;
        Vector3 moveToScale = new Vector3(1, 1, 1);

        yield return new WaitForSeconds(0.7f);
        while (true)
        {
            _transform.position = Vector3.Lerp(_transform.position, moveToPos, moveSpeed);
            _transform.localScale = Vector3.Lerp(_transform.localScale, moveToScale, 0.1f);

            if (_transform.position.y >= moveToPos.y - 0.05f && _transform.position.y <= moveToPos.y + 0.05f)
            {
                break;
            }
            yield return null;
        }

        _transform.position = originPos;
        _transform.localScale = originScale;

        _transform.gameObject.SetActive(false);
    }
}

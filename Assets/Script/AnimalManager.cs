using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    //public static AnimalManager instance;

    public float apearTime = 0f; //등장시간 카운트
    public float apearTimeMin = 2f; //등장시간 최소
    public float apearTimeMax = 5f; //등장시간 최대
    public int childCount; //동물오브젝트 개수
    public int animalsCount;
    public List<int> nowAnimals = new List<int>(); //현재 방문 중인 동물 코드

    public SeatManager[] counterSeat; //카운터 자리

    /*
    private void Awake()
    {
        instance = this;
    }*/

    private void Start()
    {
        childCount = transform.childCount;
        animalsCount = Database.instance.animals.Count;
    }

    private void Update()
    {
        apearTime -= Time.deltaTime;
        if (apearTime <= 0) CreatAnimal();
    }

    private void ResetTime()
    {
        apearTime = Random.Range(apearTimeMin, apearTimeMax);
    }

    private void CreatAnimal()
    {
        //for (int i = 0; i < childCount; i++)
        for (int i = 0; i < childCount; i++)
        {
            //현재 인덱스의 애니멀오브젝트 할당
            AnimalMovement AnimalData = transform.GetChild(i).gameObject.GetComponent<AnimalMovement>();

            //이미 입장한 동물오브젝트면 패스
            if (AnimalData.enter) continue;

            //동물캐스팅 됐으면 등장 안됐으면 패스
            AnimalData castAnimal = CastAnimal();
            if (castAnimal != null) { AnimalData.Come(castAnimal); break; }
            else break;
        }
        ResetTime();
    }
    //랜덤동물 배정
    private AnimalData CastAnimal()
    {
        int code;
        List<int> randomCodes = new List<int>();
        int i = 0;

        while (i < animalsCount)
        {
            //랜덤 동물 배정
            code = Random.Range(0, animalsCount);

            //뽑은 랜덤코드 중 중복X -> 랜덤코드가 뽑기가능한 동물 수를 넘으면 탈출
            if (! randomCodes.Contains(code))
            {
                randomCodes.Add(code);
                
                //등장조건: 1.총수익 2.무드 3.기온 4.날씨 5.호감도 6.중복

                //총수익이 조건에 안맞으면 패스
                if (State.instance.myState.totalMoney < Database.instance.GetAnimalData(code).moneyCondition) continue;
                //중복동물 있으면 패스
                if (nowAnimals.Contains(code)) continue;

                //조건 모두 만족했으면 캐스팅
                nowAnimals.Add(code); //현재 매장 내 동물 리스트에 추가
                return Database.instance.animals[code];
            }
            else i++;
        }
        ResetTime();

        //캐스팅할 동물 없으면 빈객체 반환
        return null;
    }

    //현재 매장 내 동물 리스트에서 삭제. AnimalMovement에서 호출
    public void RemoveAnimal(int _code)
    {
        nowAnimals.Remove(_code);
    }

    public void OpenMiniBalloon(bool open)
    {
        foreach(SeatManager seet in counterSeat)
        {
            if(seet.animal != null)
                seet.animal.gameObject.GetComponent<BalloonManager>().OpenOrderBalloon(open);
        }
    }
}

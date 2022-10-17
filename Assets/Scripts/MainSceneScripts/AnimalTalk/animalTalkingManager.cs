using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animalTalkingManager : MonoBehaviour
{
    public List<GameObject> listOfAnimalObject;
    public Dictionary<GameObject, string[]> sayingOfAnimal;
    public Dictionary<GameObject, AudioClip> audioOfAnimal;

    void Awake()
    {
        sayingOfAnimal = new Dictionary<GameObject, string[]>();
        audioOfAnimal = new Dictionary<GameObject, AudioClip>();
        GenerateData();
    }
    void GenerateData()
    {
        sayingOfAnimal.Add(listOfAnimalObject[0], new string[] { "안녕?\n나는 양이야","너희가 없을 때도 마을은 계속 돌아간단다.","자주 놀러와서 마을을 지켜줘."});
        sayingOfAnimal.Add(listOfAnimalObject[1], new string[] { "안녕?\n나는 오리야", "우리 애기들 귀엽지?", "얼마전에 우리 애기가 미세 플라스틱을 먹어서 걱정이 많아.",
        "분리수거가 얼마나 중요한지 사람들이 알아야할텐데..."});
        sayingOfAnimal.Add(listOfAnimalObject[2], new string[] { "안녕?\n나는 야옹 고양이야", "혹시 너 씨앗 있니?","가게에서 산 씨앗으로 원하는 식물을 심어봐!" });
        sayingOfAnimal.Add(listOfAnimalObject[3], new string[] { "안녕?\n나는 두더지야", "얼른 공장에 정화기를 설치해야 될텐데...","설치하기 전에 너희가 공기를 정화시켜줄래?",
        "내 옆에 있는 공기 청소기를 잡아서 나쁜 구름을 무찔러줘!","하늘로 뜰 준비됐지?"});
        sayingOfAnimal.Add(listOfAnimalObject[4], new string[] { "안녕?\n나는 어미오리야", "요즘 더러운 물 마셨더니\n목이 너무 아파", "수건으로 기름들 좀 닦아줘!!","그럼 물도 정화되어 내 친구 물고기도 안아플거야!!!"});
        sayingOfAnimal.Add(listOfAnimalObject[5], new string[] { "안녕?\n나는 꼬꼬댁 닭이야", "우리 농장 아저씨는 화학 비료 대신 유기질 비료를 써!","유기질 비료를 사용하면 환경을 해치지 않고 식물을 기를 수 있어~~!"});
        sayingOfAnimal.Add(listOfAnimalObject[6], new string[] { "안녕?\n나는 음매애 소야", "내 방귀 때문에 환경이 오염된다고 들었어...","너희가 대신 정화 좀 시켜줄래??" });
        sayingOfAnimal.Add(listOfAnimalObject[7], new string[] { "안녕?\n나는 꿀꿀 돼지야", "우리가 너무 답답해서 탈출했지 뭐야.", "주인 아저씨가 담배를 너무 펴서 숨을 쉬기 힘들어.",
        "담배는 온실가스를 만들어서 우리 몸과 환경 모두에 정말 안 좋아.", "앗 주인 아저씨다!\n도망가~"});
        sayingOfAnimal.Add(listOfAnimalObject[8], new string[] { "안녕?\n나는 펭귄이야.", "나는 남극에서 왔어.", "나비 효과에 대해서 아니?","지구 어디에서인가 일어난 조그만 변화로 인해 남극의 빙하가 점점 녹고 있어.",
        "자동차 대신 버스, 비행기 대신 기차를 타서 내 집인 빙하를 지켜줘!!!"});

        audioOfAnimal.Add(listOfAnimalObject[0], Resources.Load("Audios/sheep") as AudioClip);
        audioOfAnimal.Add(listOfAnimalObject[1], Resources.Load("Audios/whiteDuck") as AudioClip);
        audioOfAnimal.Add(listOfAnimalObject[2], Resources.Load("Audios/cat") as AudioClip);
        audioOfAnimal.Add(listOfAnimalObject[3], Resources.Load("Audios/mole") as AudioClip);
        audioOfAnimal.Add(listOfAnimalObject[4], Resources.Load("Audios/yellowDuck") as AudioClip);
        audioOfAnimal.Add(listOfAnimalObject[5], Resources.Load("Audios/chicken") as AudioClip);
        audioOfAnimal.Add(listOfAnimalObject[6], Resources.Load("Audios/cow") as AudioClip);
        audioOfAnimal.Add(listOfAnimalObject[7], Resources.Load("Audios/pig") as AudioClip);
        audioOfAnimal.Add(listOfAnimalObject[8], Resources.Load("Audios/penguin") as AudioClip);

    }

    public string[] GetTalk(GameObject gb)
    {
        return sayingOfAnimal[gb];
    }

    public AudioClip GetAudio(GameObject gb)
    {
        return audioOfAnimal[gb];
    }
    
}

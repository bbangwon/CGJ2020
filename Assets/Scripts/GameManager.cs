using UnityEngine;

namespace CGJ2020
{
    public class GameManager : SingletonMono<GameManager>
    {
        [Tooltip("트레뷰셋 기본 이동 속도")]
        public float trebuchetMoveSpeed;

        [Tooltip("기수 기본 이동 속도")]
        public float flagmanMoveSpeed;

        [Tooltip("에임 이동 속도")]
        public float aimMoveSpeed;        

        [Tooltip("공격 시간")]
        public float attackTime;

        [Tooltip("공격 쿨타임")]
        public float attackCooltime;

        [Tooltip("최소 사정 거리")]
        public float minAttackRange;

        [Tooltip("최대 사정 거리")]
        public float maxAttackRange;

        [Tooltip("모드 변경 시간")]
        public float trebuchetModeChangeTime;

        [Tooltip("아이템 생성 시간")]
        public float itemGenerateTime;

        [Tooltip("버프 이동 속도 증가율")]
        public float buffedMoveSpeedAmount;

        [Tooltip("버프 이동 속도 효과 시간")]
        public float buffedMoveSpeedEffectTime;

        [Tooltip("버프 사정거리 효과 시간")]
        public float buffedAttackRangeEffectTime;

        [Tooltip("화염탄 돌덩이 크기 증가율")]
        public float buffedFireballSizeAmount;

        [Tooltip("화염탄 화염 효과 시간")]
        public float buffedFireballEffectTime;
    } 
}

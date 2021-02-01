using UnityEngine;

namespace Assets.Scripts.Base
{
    class WhoYouAre
    {
        int value;

        public int Value { get => value; set => this.value = value; }

        public WhoYouAre()
        {
            if (Application.platform == RuntimePlatform.Android)
                Value = 1;
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
                Value = 2;
        }
    }
}

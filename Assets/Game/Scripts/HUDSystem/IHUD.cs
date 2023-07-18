using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.HUDSystem
{
    public interface IHUD
    {
        void Show();
        void Hide();
    }

    public abstract class BaseHUD : MonoBehaviour, IHUD
    {
        protected virtual void OnShow() { }
        protected virtual void OnHide() { }

        public void Hide()
        {
            OnHide();
            gameObject.SetActive(false);
        }

        public void Show()
        {
            OnShow();
            gameObject.SetActive(true);
        }
    }

    [System.Serializable]
    public class HUDAssetReference : AssetReferenceT<BaseHUD>
    {
        public HUDAssetReference(string guid) : base(guid)
        {
        }
    }
}

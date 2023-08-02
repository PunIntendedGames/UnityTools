using UnityEngine.UIElements;

namespace UnityEngine.AddressableAssets
{
    public class AssetReferenceScriptableObject : AssetReferenceT<ScriptableObject>
    {
        public AssetReferenceScriptableObject(string guid) : base(guid) { }
    }

    public class AssetReferencePanelSettings : AssetReferenceT<PanelSettings>
    {
        public AssetReferencePanelSettings(string guid) : base(guid) { }
    }

    public class AssetReferenceVisualTreeAsset : AssetReferenceT<VisualTreeAsset>
    {
        public AssetReferenceVisualTreeAsset(string guid) : base(guid) { }
    }
}
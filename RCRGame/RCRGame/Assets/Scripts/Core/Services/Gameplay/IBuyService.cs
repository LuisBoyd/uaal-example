using Cysharp.Threading.Tasks;

namespace Core.Services.Gameplay
{
    public interface IBuyService
    {
        void Buy(int itemID);
        bool CanBuy(int itemID);

        UniTask BuyAsync(int itemID);
        UniTask<bool> CanBuyAsync(int itemID);
        
    }
}
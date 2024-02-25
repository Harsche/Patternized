using System.Collections;

namespace StatePattern{
    public interface IPlayerState{
        IEnumerator Enter(Player player);
        void Update(Player player);
        void FixedUpdate(Player player);
        IEnumerator Exit(Player player);
    }
}
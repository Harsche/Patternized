using System.Collections;

namespace StatePattern{
    public interface IState{
        IEnumerator Enter(Player player);
        void Update(Player player);
        void FixedUpdate(Player player);
        IEnumerator Exit(Player player);
    }
}
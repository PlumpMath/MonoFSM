public class MonoStateBase<T, F> where T : MonoStateBase<T, F> {
    public F fsm;
    public virtual void Update() {
    }
    public virtual void Enter() {
    }
    public virtual void Leave() {
    }
}

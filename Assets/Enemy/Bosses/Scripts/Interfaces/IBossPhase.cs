
public interface IBossPhase
{
    void EnterPhase(BossController boss);  
    void UpdatePhase(BossController boss);
    void ExitPhase(BossController boss);
}

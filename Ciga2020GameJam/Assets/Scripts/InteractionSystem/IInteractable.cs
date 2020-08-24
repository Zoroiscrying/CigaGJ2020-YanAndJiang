
public interface IInteractable
{
    bool CanInteract { get; }
    void OnInteract();

    void OnCanInteract();

    void OnQuitInteract();
}

using Cysharp.Threading.Tasks;

public interface IInitializable
{
    public UniTask InitializeAsync();
}

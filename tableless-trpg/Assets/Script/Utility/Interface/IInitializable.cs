using Cysharp.Threading.Tasks;

public interface IInitializable
{
    UniTask InitializeAsync();
}

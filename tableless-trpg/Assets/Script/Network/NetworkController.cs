using Cysharp.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkController : SingletonMono<NetworkController>, IInitializable
{
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private UnityTransport _unityTransport;

    public UniTask InitializeAsync()
    {
        Debug.Log("NetworkController Initialized");
        return UniTask.CompletedTask;
    }
}

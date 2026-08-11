using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface ILoadingOperation 
{
    Task LoadAsync(IProgress<float> progress = null, CancellationToken token = default);   
}

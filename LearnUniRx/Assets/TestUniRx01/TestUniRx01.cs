using System;
using System.Collections;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace TestUniRx01
{
    public class TestUniRx01 : MonoBehaviour
    {
        public RawImage rawImage0;
        public RawImage rawImage1;
        public RawImage rawImage2;

        async void Start()
        {
            LoadPrefabFromResObservable<Texture2D>("texture0").Subscribe(tex => rawImage0.texture = tex);

            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            var text2 = await LoadPrefabFromResObservable<Texture2D>("texture1");
            rawImage1.texture = text2;

            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            var text3 = await LoadPrefabFromResUniTask<Texture2D>("texture2");
            rawImage2.texture = text3;
        }

        private static async UniTask<T> LoadPrefabFromResUniTask<T>(string assetName, bool isAsync = true) where T : UnityEngine.Object
        {
            if (isAsync)
                return await Resources.LoadAsync<T>(assetName) as T;
            return Resources.Load<T>(assetName);
        }

        private static IObservable<T> LoadPrefabFromResObservable<T>(string assetName, bool isAsync = true) where T : UnityEngine.Object
        {
            if (isAsync)
                return Resources.LoadAsync<T>(assetName)
                    .AsAsyncOperationObservable()
                    .Select(resourceRequest => resourceRequest.asset)
                    .OfType<UnityEngine.Object, T>();
            return Observable.Start(() => Resources.Load<T>(assetName), Scheduler.MainThread);

        }

        private IEnumerator LoadPrefabFromRes<T>(string assetName, Action<T> loaded, bool isAsync = false) where T : UnityEngine.Object
        {
            var assetObj = default(T);
            if (isAsync)
            {
                var req = Resources.LoadAsync<T>(assetName);
                yield return req;
                assetObj = req.asset as T;
            }
            else
            {
                assetObj = Resources.Load<T>(assetName);
            }
            loaded?.Invoke(assetObj);
        }
    }
}


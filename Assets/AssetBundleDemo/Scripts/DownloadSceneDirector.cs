﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DownloadSceneDirector : MonoBehaviour {

	// プログレス
	public Text notice;
	public Text count;
	public Text per;
	public Image progressImg;
	public GameObject retryBtn;

	// ダウンロード対象のアセットバンドル
	string[] bundleNames = { "unitychan_std", "unitychan_crs", "unitychan_baseassets" };

	// Use this for initialization
	void Start () {
		// デバッグ用
		#if DEBUG
//		Caching.CleanCache ();
		#endif


		// ダウンロード開始 ====

		// アセットバンドルマネージャインスタンス取得
		AssetBundleManager bundleMng = AssetBundleManager.Instance;
		// 初期値設定
		bundleMng.Initialize ("https://dl.dropboxusercontent.com/u/91930162/github/OSX/", 1);
		// ダウンロード開始
		bundleMng.DownloadAssetBundle (bundleNames, OnDownloading);

		// リトライボタンを無効化
		retryBtn.SetActive(false);
		notice.text = "NOW LOADING";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// ダウンロード実行中
	private void OnDownloading (float progress, int fileIndex, bool isComplete, string error) {
		// エラー処理
		if (error != null) {
			// リトライボタンアクティブ
			retryBtn.SetActive(true);
			notice.text = "FAILED";

			// その他無効化
			count.enabled = false;
			per.enabled = false;
			progressImg.enabled = false;

			Debug.Log("ダウンロードエラー : "+error);
		}

		// 進捗更新
		if (!isComplete) {
			// テキストプログレス更新
			int prg = (int)(progress * 100f);
			per.text = prg.ToString () + "%";
			// プログレスバー更新
			progressImg.fillAmount = progress;
			// ファイル数更新
			int index = fileIndex+1;
			count.text = index + "/" + bundleNames.Length;
		}
		else {
			// ダウンロード完了
			notice.text = "COMPLETE";
			per.text = "100%";
			progressImg.fillAmount = 1f;
			Debug.Log ("ダウンロード完了");

			// ダウンロード完了テキストを見せるための遅延処理
			StartCoroutine(WaitForScene());
		}
	}


	// 遅延処理
	IEnumerator WaitForScene() {
		// 1秒待ってから遷移
		yield return new WaitForSeconds (1f);

		// 遷移
		Application.LoadLevel ("MainScene");
	}
}

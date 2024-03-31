・hit簡単使い方

□基本
ロックマンのような、敵をすり抜けるが、
攻撃と防御判定があるようなアタリ判定の実装をする際のシステム。

マップの判定をするゲームオブジェクト、
その子オブジェクトに攻撃、防御用のオブジェクトを設定する。

サンプルシーンをつけておくので、そちらを参照。


□Unity側の操作
LayerにHit専用のレイヤーを作る
Hit同士以外は当たらないように
ProjectSettingsのPhysics2Dの設定をする

例）
project settingsを開く
>Tags and Layersで空いてる場所にHitというレイヤーを追加

project settingsを開く
>Physics2DのLayerCollisionMaskを修正
>Hit同士にチェックを入れ、他を外す


Playerオブジェクトを生成
>BoxCollider2Dをアタッチ。これは地形とのアタリ判定用。
>HitBase.cssをアタッチ
その子供としてPlayerDefオブジェクトを生成
>BoxCollider2D(トリガー）をアタッチ。これは敵などとの判定用（防御）
>HitDefData.cssをアタッチ。防御データ用スクリプト


Enemyオブジェクトを生成
>BoxCollider2Dをアタッチ。これは地形とのアタリ判定用。
>HitBase.cssをアタッチ
その子供としてEnemyAtkfオブジェクトを生成
>BoxCollider2D(トリガー）をアタッチ。これは敵などとの判定用（攻撃）
>HitAtkData.cssをアタッチ。攻撃データ用スクリプト

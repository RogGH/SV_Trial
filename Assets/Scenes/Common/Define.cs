// 言語
public enum Lang
{
    Japanese, English, Num
}

// ジョブ、略称とか全部
// ナイト           PLD	Paladin
// 戦士	戦	        WAR	Warrior
// 暗黒騎士	暗	    DRK	Dark Knight
// ガンブレイカー	GNB	Gunbreaker
// 白魔道士	白	    WHM	White Mage
// 学者	学	        SCH	Scholar
// 占星術師	占	    AST	Astrologian
// 賢者	賢	        SGE	Sage
// モンク	モ	    MNK	Monk
// 竜騎士	竜	    DRG	Dragoon
// 忍者	忍	        NIN	Ninja
// 侍	侍	        SAM	Samurai
// リーパー	リ	    RPR	Reaper
// 吟遊詩人	吟	    BRD	Bard
// 機工士	機	    MCH	Mechanist
// 踊り子	踊	    DNC	Dancer
// 召喚士	召	    SMN	Summoner
// 赤魔道士	赤	    RDM	Red Mage
// 黒魔道士	黒	    BLM	Black Mage

// 実績フラグ
public enum CharNo
{
    // キャラクター解放
    Samurai,    // 侍
    Machinist,  // 機工
    Dancer,     // 踊り子
    BlackMage,  // 黒
    Summoner,   // 召喚
    WhiteMage,  // 白

    Num
};

// ステージ番号
public enum StageNo
{
    Trial,      // 体験版
    Practice,   // 練習
    Titan,      // タイタン
    Ifrit,      // イフリート
    Garuda,     // ガルーダ
    Ultima,     // アルテマウェポン

    Num
};

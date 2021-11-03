using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankClearPanel : PanelScript
{
    public Button homeBtn;
    public Button retryBtn;
    public Button rankBtn;
    public Button backBtn;

    public Text curScoreText;
    public Text highScoreText;

    

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        homeBtn.onClick.AddListener(() => OnClickHomeBtn());
        retryBtn.onClick.AddListener(() => OnClickRetryBtn());
        rankBtn.onClick.AddListener(() => OnClickRankBtn());
        backBtn.onClick.AddListener(() => RankListPanel(false));
    }
    
  

    


}

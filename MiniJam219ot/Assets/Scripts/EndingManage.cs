using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EndingManage : MonoBehaviour
{
    public GameObject[] pages;
    public int pageCount = 0;
    [SerializeField] TMP_Text pageText;
    private void Start()
    {
        pageText.text = (pageCount + 1) + "/" + pages.Length;
    }
    public void eRB()
    {
        pages[pageCount].SetActive(false);
        pageCount++;
        pageText.text = (pageCount+1) + "/" + pages.Length;
        pages[pageCount].SetActive(true);
    }
    public void eLB()
    {
        pages[pageCount].SetActive(false);
        pageCount--;
        pageText.text = (pageCount + 1) + "/" + pages.Length;
        pages[pageCount].SetActive(true);
    }
    public void GoToMenuButton()
    {
        SceneManager.LoadScene(1);
    }
}

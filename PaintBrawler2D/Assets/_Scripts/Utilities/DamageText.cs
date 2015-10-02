using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {
    [SerializeField]
    private float _showTimer = 1f;
    [SerializeField]
    private float _speed = 0.2f;
    private float _fadeSpeed = 0.03f;
    [SerializeField]
    private GameObject[] _textFields;
    [SerializeField]
    private Color[] ColorArray = { new Color(217f/255f, 40f/255f, 46f/255f),
                                   new Color(255f/255f, 209f/255f, 64/255f),
                                   new Color (57f/255f, 212f/255f, 50f/255f) };

    public void Initialize(int Damage, string SetColor)
    {

        Color color = new Color(1f, 1f, 1f);
        switch (SetColor)
        {
            case "Red":
                color = ColorArray[0];
                break;
            case "Yellow":
                color = ColorArray[1];
                break;
            case "Green":
                color = ColorArray[2];
                break;
        }
        foreach(GameObject x in _textFields)
        {
            x.GetComponent<Text>().color = color;
            if (SetColor == "Green")
            {
                x.GetComponent<Text>().text = "+" + Damage.ToString();
            }
            else
            {
                x.GetComponent<Text>().text = Damage.ToString();
            }
        }
    }
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(0f, 1f, 0f) * 0.01f;
        _showTimer -= Time.deltaTime;
        if (_showTimer < 0) { 
            foreach (GameObject x in _textFields)
            {
                x.GetComponent<Text>().color -= new Color(0f, 0f, 0f, 1f) * _fadeSpeed;

                if (x.GetComponent<Text>().color.a < 0.05f)
                {
                    Destroy(gameObject);
                }
            }
        }
	}
}

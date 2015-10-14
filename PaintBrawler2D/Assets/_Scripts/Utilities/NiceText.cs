using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NiceText : MonoBehaviour {
    [SerializeField]
    private float _showTimer = 1f;
    [SerializeField]
    private float _speed = 0.2f;
    private float _fadeSpeed = 0.03f;
    [SerializeField]
    private GameObject[] _textFields;
    private Color[] ColorArray = { new Color(217f/255f, 40f/255f, 46f/255f),
                                   new Color(234f/255f, 123f/255f, 54f/255f),
                                   new Color(255f/255f, 209f/255f, 64/255f),
                                   new Color (57f/255f, 212f/255f, 50f/255f),
                                   new Color (43f/255f, 125f/255f, 225f/255f),
                                   new Color (130f/255f, 83f/255f, 137f/255f)};

    // 0 - bad sound, 1 - good sound, 2 - great sound
    [SerializeField]
    private AudioClip[] _audioSounds;
    [SerializeField]
    private AudioSource _audio;

    void Awake() {
        _audio = GetComponent<AudioSource>();
    }

    public void Initialize(string SetColor, string SetString, int soundIndex)
    {

        Color color = new Color(1f, 1f, 1f);
        switch (SetColor)
        {
            case "Red":
                color = ColorArray[0];
                break;
            case "Orange":
                color = ColorArray[1];
                break;
            case "Yellow":
                color = ColorArray[2];
                break;
            case "Green":
                color = ColorArray[3];
                break;
            case "Blue":
                color = ColorArray[4];
                break;
            case "Purple":
                color = ColorArray[5];
                break;
        }
        
        _audio.PlayOneShot(_audioSounds[soundIndex], 0.4f);

        _textFields[1].GetComponent<Text>().color = color;

        foreach(GameObject x in _textFields) {
            x.GetComponent<Text>().text = SetString;
        }
    }
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(0f, 1f, 0f) * _speed;
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

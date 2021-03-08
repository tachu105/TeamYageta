using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyText : MonoBehaviour
{
    private Slider difficultySlider;
    [SerializeField] private Text difficultyText;
    int difficuntyValue;
    // Start is called before the first frame update
    void Start()
    {
        difficultySlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        difficuntyValue = (int)difficultySlider.value;
        if (difficuntyValue == 1) difficultyText.text = "Easy";
        else if (difficuntyValue == 2) difficultyText.text = "Normal";
        else if (difficuntyValue == 3) difficultyText.text = "Hard";
    }
}

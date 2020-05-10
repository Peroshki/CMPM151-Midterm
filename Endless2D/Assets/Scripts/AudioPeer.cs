using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// create this required component....
[RequireComponent (typeof (AudioSource))]

public class AudioPeer : MonoBehaviour 
{
    private float delTime;

	// need to instantiate an audio source and array of samples to store the fft data.
	[Range(0.01f, 1f)]
    public float deltaUpdate;
	public int numPoints;
	public float height;

	public static AudioSource _audioSource;
	public static float[] spectrumData = new float[512];
	public static float interpolationPos;
	public static float[] cleansedData = {0.0f};

	// Useful info for other programs
	public static int maxDataIndex;
    public static Color colorOffset;


	// Use this for initialization
	void Start () {
		
		_audioSource = GetComponent<AudioSource> ();	
	}
	
	// Update is called once per frame
	void Update () {

		GetSpectrumAudioSource ();
		updateFFT();

	}


	void GetSpectrumAudioSource()
	{
		// this method computes the fft of the audio data, and then populates spectrumData with the spectrum data.
		_audioSource.GetSpectrumData (spectrumData, 0, FFTWindow.Hanning);
	}

	void updateFFT() {
        delTime += Time.deltaTime;
        if (delTime >= deltaUpdate) {
			print("Updating");
            delTime = 0;
            calculateFFTSizes(numPoints, height);
            colorOffset = Color.HSVToRGB((float)maxDataIndex / (float)numPoints, 1.0f, 0.5f);
        }
		interpolationPos = delTime / deltaUpdate;
    }

	public static void calculateFFTSizes(int numPartitions, float scale) 
	{
        // animate the cube size based on sample data
		cleansedData = new float[numPartitions];
		float partitionIndx = 0;
		int numDisplayedBins = 512 / 2; //NOTE: we only display half the spectral data because the max displayable frequency is Nyquist (at half the num of bins)

		for (int i = 0; i < numDisplayedBins; i++) 
		{
			if(i < numDisplayedBins * (partitionIndx + 1) / numPartitions){
				cleansedData[(int)partitionIndx] += spectrumData [i] / (512/numPartitions);
			}
			else{
				partitionIndx++;
				i--;
			}
		}

        maxDataIndex = 0;
		// scale and bound the average magnitude.
		for(int i = 0; i < numPartitions; i++)
		{
			cleansedData[i] = cleansedData[i]*scale;

            float abs = Mathf.Abs(cleansedData[i]);
            if (abs > cleansedData[maxDataIndex]) maxDataIndex = i;

			if (cleansedData[i] > scale) cleansedData[i] = scale;
		}
    }
}



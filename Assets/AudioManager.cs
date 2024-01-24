using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] sesDosyalari;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
         // AudioSource bileşenini al
        audioSource = GetComponent<AudioSource>();

        // 10 ses dosyasını diziye ata
        sesDosyalari = new AudioClip[48];
        for (int i = 0; i < 48; i++)
        {
            // "SesDosyasi" adında bir klasör içindeki ses dosyalarını sırayla atıyoruz. İsterseniz bu yolu değiştirebilirsiniz.
            sesDosyalari[i] = Resources.Load<AudioClip>("sesler/ses " + (i + 1));
        }

        // İlk ses dosyasını oynat - Merhaba sesi vs... koyulabilir.
        Oynat(0);
    }

    // Update is called once per frame
    void Update()
    {
        // Örneğin, herhangi bir tuşa basıldığında bir sonraki sesi oynat
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            int rastgeleSesIndex = Random.Range(0, sesDosyalari.Length);
            Oynat(rastgeleSesIndex);
        }
        */
    }

    void Oynat(int index)
    {
        // Belirli bir indeksteki sesi oynat
        audioSource.clip = sesDosyalari[index];
        audioSource.Play();
    }
}

using Audio;
using UnityEngine;
using UnityEngine.Audio;

public class SimpleMovement : MonoBehaviour
{
    public AudioClip TestAudioClip;
    public AudioMixerGroup MixerGroup;
    
    public float Speed = 1;

    private void Update()
    {
        if (GlobalGameState.IsUnpaused())
        {
            var horizontalAxis = Input.GetAxisRaw("Horizontal");
            var verticalAxis = Input.GetAxisRaw("Vertical");

            var movementVector = new Vector3(horizontalAxis, 0, verticalAxis).normalized * Time.deltaTime * Speed;
            transform.Translate(movementVector, Space.Self);

            if (Input.GetKeyUp(KeyCode.Space))
            {
                GetComponent<SoundManager>().PlaySound(TestAudioClip, MixerGroup);
            }
        }
    }
}

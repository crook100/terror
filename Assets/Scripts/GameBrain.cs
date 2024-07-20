using UnityEngine;

public class GameBrain : MonoBehaviour
{
    readonly float CONFIG_DEFAULT_VOLUME = 1f;
    readonly Vector2 CONFIG_DEFAULT_PITCHMINMAX = new Vector2(0.8f, 1.2f);

    public float random_footstep_timer = 5f;
    public Transform random_footsteps_object;
    private float random_footstep_timer_counter = 0f;

    [Range(0, 100)]
    public int random_footstep_chance = 25;

    [System.Serializable]
    public struct GroundType
    {
        public string name;
        public AudioClip[] sounds;
    }

    [SerializeField]
    public GroundType[] groundTypes;

    public AudioClip[] getFootstepsByGroundType(int id)
    {
        try
        {
            return groundTypes[id].sounds;
        }catch
        {
            return null;
        }
    }

    public void Update()
    {
        random_footstep_timer_counter -= Time.deltaTime;

        if (random_footstep_timer_counter <= 0) 
        {
            random_footstep_timer_counter = random_footstep_timer;
            AttemptFootstepSpawn();
        }
    }

    public void AttemptFootstepSpawn() 
    {
        if(Random.Range(0, 100) <= random_footstep_chance) 
        {
            //Spawn
            Instantiate(random_footsteps_object, new Vector3(Random.Range(-25, 25), 0, Random.Range(-25, 25)), Quaternion.identity);
        }
    }

    public void PlaySoundAt(AudioClip sound, Vector3 pos, Vector2? pitchMinMax = null, float? volume = null)
    {
        if (!pitchMinMax.HasValue) pitchMinMax = CONFIG_DEFAULT_PITCHMINMAX;
        if (!volume.HasValue) volume = CONFIG_DEFAULT_VOLUME;

        GameObject obj = new GameObject(sound.name, new System.Type[0]);
        var autoDestroy = obj.AddComponent<AutoDestroy>();
        autoDestroy.RemoteDestroy(sound.length);
        var audioSource = obj.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.Stop();

        audioSource.clip = sound;
        audioSource.loop = false;
        audioSource.volume = volume.Value;
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 0f;
        audioSource.maxDistance = 50f;
        audioSource.pitch = Random.Range(pitchMinMax.Value.x, pitchMinMax.Value.y);
        audioSource.rolloffMode = AudioRolloffMode.Linear;

        obj.transform.position = pos;
        audioSource.Play();
    }
}

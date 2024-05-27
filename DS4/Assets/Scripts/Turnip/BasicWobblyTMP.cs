using TMPro;
using UnityEngine;

namespace _BasicWobblyTMP.Scripts
{
    public class BasicWobblyTMP : MonoBehaviour
    {
        [Header("Editable")]
        [SerializeField] private WobblyTextType _Preset;
        [SerializeField, Range(0f, 30f)] private float _WobblePower;
        [Header("Special")]
        public bool Special1;
        private TMP_Text _TextMeshPro;
        private Mesh _Mesh;
        private Vector3[] _Vertices;
        private int[] _WordIndexes;
        private int[] _WordLengths;

        private void Awake()
        {
            _TextMeshPro = GetComponent<TMP_Text>();
            InitializeWordIndexes();
        }

        private void Update()
        {
            if(Special1)
            {
                if (transform.parent.localScale != Vector3.one) Wobble();
            }
            else Wobble();
        }
        void Wobble()
        {
            _TextMeshPro.ForceMeshUpdate();
            ApplyEffect();
        }

        private void ApplyEffect()
        {
            switch (_Preset)
            {
                case WobblyTextType.WaveOne:
                    ApplyWaveEffectOne();
                    break;
                case WobblyTextType.WaveTwo:
                    ApplyWaveEffectTwo();
                    break;
                case WobblyTextType.WaveThree:
                    ApplyWaveEffectThree();
                    break;
                case WobblyTextType.WaveFour:
                    ApplyWaveEffectFour();
                    break;
            }
        }

        private void ApplyWaveEffectOne()
        {
            var textInfo = _TextMeshPro.textInfo;
            for (var i = 0; i < textInfo.characterCount; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible)
                    continue;

                _Vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
                for (var j = 0; j < 4; j++)
                {
                    var orig = _Vertices[charInfo.vertexIndex + j];
                    _Vertices[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 2 + orig.x * 0.01f) * _WobblePower, 0);
                }
            }

            for (var i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                _TextMeshPro.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }
        }

        private void ApplyWaveEffectTwo()
        {
            _Mesh = _TextMeshPro.mesh;
            _Vertices = _Mesh.vertices;

            for (var i = 0; i < _Vertices.Length; i++)
            {
                var offset = CalculateOffset(Time.time + i);
                _Vertices[i] += offset;
            }

            _Mesh.vertices = _Vertices;
            _TextMeshPro.canvasRenderer.SetMesh(_Mesh);
        }

        private void ApplyWaveEffectThree()
        {
            _Mesh = _TextMeshPro.mesh;
            _Vertices = _Mesh.vertices;

            for (var i = 0; i < _TextMeshPro.textInfo.characterCount; i++)
            {
                var charInfo = _TextMeshPro.textInfo.characterInfo[i];
                var index = charInfo.vertexIndex;
                var offset = CalculateOffset(Time.time + i * _WobblePower);

                for (var j = 0; j < 4; j++)
                {
                    _Vertices[index + j] += offset;
                }
            }

            _Mesh.vertices = _Vertices;
            _TextMeshPro.canvasRenderer.SetMesh(_Mesh);
        }

        private void ApplyWaveEffectFour()
        {
            _Mesh = _TextMeshPro.mesh;
            _Vertices = _Mesh.vertices;

            for (var w = 0; w < _WordIndexes.Length; w++)
            {
                var wordIndex = _WordIndexes[w];
                var offset = CalculateOffset(Time.time + w * _WobblePower);

                for (var i = 0; i < _WordLengths[w]; i++)
                {
                    var characterInfo = _TextMeshPro.textInfo.characterInfo[wordIndex + i];
                    var index = characterInfo.vertexIndex;

                    for (var j = 0; j < 4; j++)
                    {
                        _Vertices[index + j] += offset;
                    }
                }
            }

            _Mesh.vertices = _Vertices;
            _TextMeshPro.canvasRenderer.SetMesh(_Mesh);
        }

        private Vector3 CalculateOffset(float time)
        {
            return new Vector3(Mathf.Sin(time * 3.3f), Mathf.Cos(time * _WobblePower), 0);
        }

        /// <summary>
        /// Sets the text of the TMP component and initializes word indexes for wavy effects.
        /// </summary>
        /// <param name="text">The text to set.</param>
        public void SetText(string text)
        {
            _TextMeshPro.text = text;
            InitializeWordIndexes();
        }

        private void InitializeWordIndexes()
        {
            var text = _TextMeshPro.text;
            var wordCount = text.Split(' ').Length;
            _WordIndexes = new int[wordCount];
            _WordLengths = new int[wordCount];

            var wordStartIndex = 0;
            var wordIndex = 0;

            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] != ' ' && i != text.Length - 1) continue;
                _WordIndexes[wordIndex] = wordStartIndex;
                _WordLengths[wordIndex] = i - wordStartIndex + (i == text.Length - 1 && text[i] != ' ' ? 1 : 0);
                wordStartIndex = i + 1;
                wordIndex++;
            }
        }

        private enum WobblyTextType
        {
            WaveOne,
            WaveTwo,
            WaveThree,
            WaveFour
        }
    }
}

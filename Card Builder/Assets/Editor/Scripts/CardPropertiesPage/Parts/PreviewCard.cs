using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardBuilder
{
    public class PreviewCard : ScriptableObject
    {

        GameObject canvasPrefab = null;
        GameObject newCamObject;
        GameObject currentObj = null;
        Camera camera;

        Card tempCard;
        IUpdateCard cardToUpdate;

        [SerializeField]
        public RenderTexture render;
        Image backgroundImage;

        VisualElement previewElement;

        public void Initialize(VisualElement previewElement)
        {

            this.previewElement = previewElement;

            backgroundImage = new();
            backgroundImage.image = render;
            previewElement.Add(backgroundImage);
        }

        public void SetupCamera()
        {
            newCamObject = new GameObject("Card Builder Camera");
            newCamObject.hideFlags = HideFlags.HideAndDontSave;
            camera = newCamObject.AddComponent<Camera>();
            camera.targetTexture = render;
            camera.farClipPlane = 2;
            camera.nearClipPlane = 0.3f;
            camera.enabled = false;
            camera.useOcclusionCulling = false;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.clear;
          //  camera.cameraType = CameraType.Preview;

            //Sent it to space, so nothing else is in frame
            newCamObject.transform.position = Vector3.one * 1000;

        }

        public void OnGUI()
        {
            if (camera == null) SetupCamera();
            camera.Render();
        }


        public void ChangeValue(object sender, ChangedValue evt)
        {



            string valueName = Helpers.StreamWriterMethods.ConvertPropertyToLine(evt.valueName);



            foreach (var info in tempCard.GetType().GetFields())
            {
                if (valueName != info.Name) continue;


                switch(info.FieldType.ToString())
                {
                    case "System.Int32":
                        info.SetValue(tempCard, int.Parse(evt.newValue));
                        break;
                    case "System.String":
                        info.SetValue(tempCard, evt.newValue);
                        break;
                    case "UnityEngine.Sprite":
                        info.SetValue(tempCard, Helpers.IOMethods.GetObjectFromGUID<Sprite>(evt.newValue));
                        break;
                    default:
                        Type enumType = info.GetValue(tempCard).GetType();

                        if (Enum.IsDefined(enumType, evt.newValue))
                        {
                            Enum enumValue = (Enum)Enum.Parse(enumType, evt.newValue);

                            info.SetValue(tempCard, enumValue);
                        }
                        else Logs.Warning("Invalid Enum Value");
                        break;

                }

            }

            cardToUpdate.UpdateCard();



        }

        public void Remove()
        {
            DestroyImmediate(currentObj);
            camera.Render();
            DestroyImmediate(newCamObject);
            tempCard = null;
            cardToUpdate = null;
            canvasPrefab = null;
        }

        public void OnDestroy()
        {
            DestroyImmediate(newCamObject);
        }


        //Is being called using reflection, as the generic is passed by string
        public void NewPrefab<T>(SavedCardDataEditor savedCard) where T : Card
        {



            if (newCamObject == null) SetupCamera();
            if (currentObj != null) DestroyImmediate(currentObj);
            canvasPrefab = savedCard.TemplatePrefab;
            tempCard = Instantiate(savedCard.card);

            currentObj = Instantiate(canvasPrefab, new Vector3(0, 0, 1), Quaternion.identity, newCamObject.transform);

            currentObj.transform.localPosition = new Vector3(0, 0, 1);
            currentObj.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);

            ICardCanvas<T> currentCardCanvas = currentObj.GetComponent<ICardCanvas<T>>();


            Debug.Log(tempCard.name);

            currentCardCanvas.ConnectData((T)tempCard);

            cardToUpdate = currentObj.GetComponent<IUpdateCard>();

            return;
        }




    }
}

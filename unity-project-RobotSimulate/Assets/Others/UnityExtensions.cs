using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class UnityExtensions
{
    #region Transform Extension
    public static T GetOrAddComponent<T>(this Transform trans) where T : Component
    {
        T value = trans.GetComponent<T>();
        if (value == null)
        {
            value = trans.gameObject.AddComponent<T>();
        }
        return value;
    }
    #endregion

    #region GameObject Extension
    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
    {
        T value = obj.GetComponent<T>();
        if (value == null)
        {
            value = obj.AddComponent<T>();
        }
        return value;
    }
    
    public static void UpdateActive(this GameObject gameObject, bool isActive)
    {
        if(gameObject.activeSelf != isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
    
    public static void UpdateActive(this Component component, bool isActive)
    {
        if(component.gameObject.activeSelf != isActive)
        {
            component.gameObject.SetActive(isActive);
        }
    }
    #endregion

    #region CanvasGroup Extension
    public static void Disable(this CanvasGroup canvasGroup)
    {
        if(canvasGroup == null)
            return;
        
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    
    public static void Enable(this CanvasGroup canvasGroup)
    {
        if(canvasGroup == null)
            return;
        
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    
    public static void SetData(this CanvasGroup canvasGroup, float alpha, bool interactable, bool blocksRaycasts)
    {
        if(canvasGroup == null)
            return;
        
        canvasGroup.alpha = alpha;
        canvasGroup.interactable = interactable;
        canvasGroup.blocksRaycasts = blocksRaycasts;
    }



    #endregion

    #region String Extension
    public static bool IsNotNullOrEmpty(this string str) => !string.IsNullOrEmpty(str);
    public static string EscapeURL(this string url) => UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    public static byte[] Convert2Bytes(this string str) => Encoding.UTF8.GetBytes(str);
    public static string Convert2String(this byte[] datas) => Encoding.UTF8.GetString(datas);
    #endregion
    
    #region Texture2D Extension
    public static Sprite Convert2Sprite(this Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }
    #endregion

    #region RectTransform Extension

    public static Bounds GetBonuds(this RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        Vector3 min = corners[0];
        Vector3 max = corners[0];
        
        //找出最小和最大角
        for (int i = 0; i < 4; i++)
        {
            min = Vector3.Min(min, corners[i]);
            max = Vector3.Max(max, corners[i]);
        }
        
        Vector3 center = (min + max) * 0.5f;
        Vector3 size = max - min;
        return new Bounds(center, size);
    }

    #endregion
    
    public static void SetAnchors(this RectTransform This, Vector2 AnchorMin, Vector2 AnchorMax)
    {
        var OriginalPosition = This.localPosition;
        var OriginalSize = This.sizeDelta;

        This.anchorMin = AnchorMin;
        This.anchorMax = AnchorMax;

        This.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, OriginalSize.x);
        This.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, OriginalSize.y);
        This.localPosition = OriginalPosition;
    }
    
    public static float GetAspectRatio(this Sprite sprite)
    {
        return sprite.rect.width / sprite.rect.height;
    }
}

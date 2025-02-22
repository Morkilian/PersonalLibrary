using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Used exclusively to paste informative text
/// </summary>
public class Comment : MonoBehaviour
{
    [TextArea(10,30)]public string comment;
}

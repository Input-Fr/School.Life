using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class NetworkManagerUI : MonoBehaviour
{
	[SerializeField]private Button hostBtn;
	[SerializeField]private Button clientBtn;
	[SerializeField]private Button QuickJoinBtn;
	[SerializeField]private Camera cam;
	[SerializeField]private GameObject inputField;
	[SerializeField]private GameObject inputFieldToDisable;
        
	private void Awake()
	{
		hostBtn.onClick.AddListener(() => { 
											hostBtn.gameObject.SetActive(false);
											clientBtn.gameObject.SetActive(false);
											inputFieldToDisable.gameObject.SetActive(false);
											testRelay.Instance.CreateRelay();
											cam.gameObject.SetActive(false);
											
											 });
		clientBtn.onClick.AddListener(() => { cam.gameObject.SetActive(false);
											  string catchInput = inputField.GetComponent<Text>().text;
											  testRelay.Instance.JoinRelay(catchInput);
											  hostBtn.gameObject.SetActive(false);
											  clientBtn.gameObject.SetActive(false); 
											  inputFieldToDisable.gameObject.SetActive(false); 
											  });
		QuickJoinBtn.onClick.AddListener(() => { cam.gameObject.SetActive(false);
											  });
	}

	
}

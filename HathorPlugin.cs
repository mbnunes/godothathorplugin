using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Text;

public class HathorPlugin : CanvasLayer
{

	private string url = "localhost";
	private string urlBase = "http://localhost:8000";
	private string tokenUid = "";
	private string passphrase = "";
	private Error err;

	private TextEdit txtEditConsole;
	private string wallet_id = "my_wallet";
	private string seedKey = "default";


	public override void _Ready()
	{
		GetNode("HTTPRequest").Connect("request_completed", this, "OnRequestCompleted");
		txtEditConsole =  GetNode<TextEdit>("txtEditConsole");        

		GetNode("btnStartWallet").Connect("pressed", this, "OnButtonStartWalletPressed");
		GetNode("btnStatusWallet").Connect("pressed", this, "OnButtonStatusWalletPressed");
		GetNode("btnBalance").Connect("pressed", this, "OnButtonBalancePressed");
		GetNode("btnCurrentAddress").Connect("pressed", this, "OnButtonCurrentAddressPressed");
		GetNode("btnAddresses").Connect("pressed", this, "OnButtonAddressesPressed");
		GetNode("btnSimpleSendTx").Connect("pressed", this, "OnButtonSimpleSendTxPressed");
		GetNode("btnCreateToken").Connect("pressed", this, "OnButtonCreateTokenPressed");
	}

	public void OnButtonStartWalletPressed()
	{
		Godot.Collections.Dictionary bodyDict = new Godot.Collections.Dictionary();
		bodyDict.Add("wallet-id", wallet_id);
		bodyDict.Add("seedKey", seedKey);

		if (passphrase != "")
		{
			bodyDict.Add("passphrase", passphrase);
		}

		string query = JSON.Print(bodyDict);

		HTTPRequest httpRequest = GetNode<HTTPRequest>("HTTPRequest");
		string[] headers = new string[] { "Content-Type: application/json" };
		httpRequest.Request(urlBase + "/start", headers, false, HTTPClient.Method.Post, query);
	}

	public void OnButtonStatusWalletPressed()
	{
		string[] headers = new string[] { "x-wallet-id: " + wallet_id };

		HTTPRequest httpRequest = GetNode<HTTPRequest>("HTTPRequest");
		httpRequest.Request(urlBase + "/wallet/status", headers);

	}

	public void OnButtonBalancePressed()
	{
		string[] headers = new string[] { "x-wallet-id: " + wallet_id };

		if (tokenUid != "")
		{
			HTTPRequest httpRequest = GetNode<HTTPRequest>("HTTPRequest");
			httpRequest.Request(urlBase + " /wallet/balance?token=" + tokenUid, headers);
		}
		else
		{
			HTTPRequest httpRequest = GetNode<HTTPRequest>("HTTPRequest");
			httpRequest.Request(urlBase + "/wallet/balance", headers);
		}

	}

	public void OnButtonCurrentAddressPressed()
	{
		string[] headers = new string[] { "x-wallet-id: " + wallet_id };

		bool mark_as_used = false;
		int index = -1;		

		string query = "";

		if (mark_as_used && index >= 0){
			query = "?mark_as_used="+mark_as_used;
		}else if (mark_as_used && index >= 0){
			query = "?mark_as_used="+mark_as_used+"&index="+index.ToString();
		}else if (!mark_as_used && index >= 0){
			query = "?index="+index.ToString();
		}
		
		HTTPRequest httpRequest = GetNode<HTTPRequest>("HTTPRequest");

		if (query != ""){
			httpRequest.Request(urlBase + "/wallet/address"+query, headers);
		}else {
			httpRequest.Request(urlBase + "/wallet/address", headers);
		}
		

	}
	
	public void OnButtonAddressIndexPressed()
	{
		string[] headers = new string[] { "x-wallet-id: " + wallet_id };
		
		string address = "";

		HTTPRequest httpRequest = GetNode<HTTPRequest>("HTTPRequest");
		httpRequest.Request(urlBase + "/wallet/address-index?address=" + address, headers);
	}
	
	public void OnButtonAddressesPressed()
	{
		string[] headers = new string[] { "x-wallet-id: " + wallet_id };

		HTTPRequest httpRequest = GetNode<HTTPRequest>("HTTPRequest");
		httpRequest.Request(urlBase + "/wallet/addresses", headers);
	}	

	public void OnButtonSimpleSendTxPressed()
	{
		string address = "";
		int value = 0;
		string token = "";
		string change_address = "";
		
		string[] headers = new string[] { "Content-Type: application/json", "x-wallet-id: " + wallet_id };
		
		Godot.Collections.Dictionary bodyDict = new Godot.Collections.Dictionary();
		bodyDict.Add("wallet-id", wallet_id);
		bodyDict.Add("seedKey", seedKey);

		if (token != "")
		{
			bodyDict.Add("token", token);
		}
		
		if (change_address != "")
		{
			bodyDict.Add("change_address", change_address);
		}

		string query = JSON.Print(bodyDict);

		HTTPRequest httpRequest = GetNode<HTTPRequest>("HTTPRequest");
		
		httpRequest.Request(urlBase + "/start", headers, false, HTTPClient.Method.Post, query);
	}
	
	// public void OnButtonSendTxPressed()
	// {
	// 	string[] outputs = new string[] {};
	// 	string[] inputs = new string[] {};
		
	// 	//
	// 	int value = 0;
	// 	string token = "";
		
	// 	Godot.Collections.Dictionary itemOutput = new Godot.Collections.Dictionary();
	// 	itemOutput.Add("address", wallet_id);
	// 	itemOutput.Add("value", value);
		
	// 	if(token != ""){
	// 		itemOutput.Add("token", token);
	// 	}
		
	// 	outputs.Add(JSON.Print(itemOutput));
		
	// 	Godot.Collections.Dictionary itemInput = new Godot.Collections.Dictionary();
		
				
	// 	string[] headers = new string[] { "Content-Type: application/json", "x-wallet-id: " + wallet_id };
		
	// 	Godot.Collections.Dictionary bodyDict = new Godot.Collections.Dictionary();
	// 	bodyDict.Add("wallet-id", wallet_id);
	// 	bodyDict.Add("seedKey", seedKey);

	// 	if (token != "")
	// 	{
	// 		bodyDict.Add("token", token);
	// 	}
		
	// 	if (change_address != "")
	// 	{
	// 		bodyDict.Add("change_address", change_address);
	// 	}

	// 	string query = JSON.Print(bodyDict);

	// 	HTTPRequest httpRequest = GetNode<HTTPRequest>("HTTPRequest");
		
	// 	httpRequest.Request(urlBase + "/start", headers, false, HTTPClient.Method.Post, query);
	// }

	public void OnButtonCreateTokenPressed()
	{
		string name = "";
		string symbol = "";
		string address = "";
		int amount = 0;
		string change_address = "";
		
		string[] headers = new string[] { "Content-Type: application/json", "x-wallet-id: " + wallet_id };
		
		Godot.Collections.Dictionary bodyDict = new Godot.Collections.Dictionary();
		bodyDict.Add("name", name);
		bodyDict.Add("symbol", symbol);
		bodyDict.Add("amount", amount);

		if (address != "")
		{
			bodyDict.Add("address", token);
		}
		
		if (change_address != "")
		{
			bodyDict.Add("change_address", change_address);
		}

		string query = JSON.Print(bodyDict);

		HTTPRequest httpRequest = GetNode<HTTPRequest>("HTTPRequest");
		
		httpRequest.Request(urlBase + "/wallet/create-token", headers, false, HTTPClient.Method.Post, query);
	}

	public void OnRequestCompleted(int result, int response_code, string[] headers, byte[] body)
	{
		JSONParseResult json = JSON.Parse(Encoding.UTF8.GetString(body));
		GD.Print(json.Result);
		//txtEditConsole.setShowLineNumbers(true);
		txtEditConsole.Text = json.Result.ToString();
	}

}

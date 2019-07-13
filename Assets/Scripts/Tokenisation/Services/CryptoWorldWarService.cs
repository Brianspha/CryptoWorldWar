//-----------------------------------------
//CryptoWorldWarService.cs
//-----------------------------------------
using System;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts;
using Nethereum.JsonRpc.UnityClient;
using Assets.Tokenisation.Helpers;
using System.Collections;
using UnityEngine;
using Nethereum.Hex.HexTypes;
using System.Numerics;
using Nethereum.ABI.Encoders;
using Nethereum.Signer;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Collections.Generic;

public class CryptoWorldWarService
{
    public Contract CryptoWorldWarContract
    {
        get;
    }
    public EthCallUnityRequest EthCallUnityRequest
    {
        get;
    }
    public TransactionSignedUnityRequest TransactionSignedUnityRequest
    {
        get;
    }
    public Exception Exception
    {
        get;
        private set;
    }
    public TransactionReceiptPollingRequest TransactionReceiptPollingRequest
    {
        get;
    }
    public CryptoWorldWarService()
    {
        TransactionSignedUnityRequest = new TransactionSignedUnityRequest(Variables.NodeAddress, Variables.PrivateKey);
        EthCallUnityRequest = new EthCallUnityRequest(Variables.NodeAddress);
        TransactionReceiptPollingRequest = new TransactionReceiptPollingRequest(Variables.NodeAddress);
        CryptoWorldWarContract = new Contract(null, Variables.ABI, Variables.ContractAddress);
    }
    public CallInput GetRegisterUserFunctionInput()
    {
        return CryptoWorldWarContract.GetFunction("registerPlayer").CreateCallInput();
    }
    public Function GetRegisterUserFunction()
    {
        return CryptoWorldWarContract.GetFunction("registerPlayer");
    }

    public CallInput GetAddTokenFunctionInput(int value, string description)
    {
        return CryptoWorldWarContract.GetFunction("mintNewCollectible").CreateCallInput(new object[] {
   value,
   description
  });
    }
    public CallInput GetAddTokenFunctionInput()
    {
        return CryptoWorldWarContract.GetFunction("mintNewCollectible").CreateCallInput();
    }
    public Function GetAddTokenFunction()
    {
        return CryptoWorldWarContract.GetFunction("mintNewCollectible");
    }
    public CallInput GetAllCollectiblesCountFunctionInput()
    {
        return CryptoWorldWarContract.GetFunction("getAllCollectibleKeys").CreateCallInput();
    }
    public Function GetAllCollectiblesCountFunction()
    {
        return CryptoWorldWarContract.GetFunction("getAllCollectibleKeys");
    }
    public CallInput GetGetCollectibleFunctionInput(int id)
    {
        return CryptoWorldWarContract.GetFunction("getAllCollectibleKeys").CreateCallInput(new object[] {
   id
  });
    }
    public Function GetGetCollectibleFunction()
    {
        return CryptoWorldWarContract.GetFunction("getCollectibelDetails");
    }

    public bool DecodeRegistrationOutput(string result)
    {
        var

        function = GetRegisterUserFunction();
        return function.DecodeSimpleTypeOutput<bool>(result);
    }
    //Check if the user top score has changed on the contract chain every 2 seconds
    public IEnumerator RegisterPlayer()
    {
        Debug.Log("Got here");
        var regFunc = new RegisterPlayerFunction();
        regFunc.FromAddress = Variables.FromAddress;
        regFunc.Gas = 8000000;
        regFunc.GasPrice = 9000000000000;
        //Call request sends and yield for response	
        yield
        return TransactionSignedUnityRequest.SignAndSendTransaction(regFunc, Variables.ContractAddress);
        //Each request has a exception and a result. The exception is set when an error occurs.
        //Follows a similar patter to the www and unitywebrequest
        if (TransactionSignedUnityRequest.Exception == null)
        {
            //var registered = DecodeRegistrationOutput(TransactionSignedUnityRequest.Result);
            Debug.Log("Results from registration: " + TransactionSignedUnityRequest.Result);
        }
        else
        {
            Debug.Log(TransactionSignedUnityRequest.Exception.ToString());
        }
    }
    public partial class GetCollectibelDetailsFunction : GetCollectibelDetailsFunctionBase { }

    [Function("getCollectibelDetails", typeof(GetCollectibelDetailsOutputDTO))]
    public class GetCollectibelDetailsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "collectibleId", 1)]
        public virtual BigInteger CollectibleId { get; set; }
    }

    public partial class NameFunction : NameFunctionBase { }

    [Function("name", "string")]
    public class NameFunctionBase : FunctionMessage
    {

    }

    public partial class TransferCollectibleFunction : TransferCollectibleFunctionBase { }

    [Function("transferCollectible", "bool")]
    public class TransferCollectibleFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "collectibleId", 1)]
        public virtual BigInteger CollectibleId { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
    }

    public partial class UpdateMaxCollectibleLevelFunction : UpdateMaxCollectibleLevelFunctionBase { }

    [Function("updateMaxCollectibleLevel", "bool")]
    public class UpdateMaxCollectibleLevelFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "newlevel", 1)]
        public virtual BigInteger Newlevel { get; set; }
    }

    public partial class GetAllCollectibleKeysFunction : GetAllCollectibleKeysFunctionBase { }

    [Function("getAllCollectibleKeys", "uint256[]")]
    public class GetAllCollectibleKeysFunctionBase : FunctionMessage
    {

    }

    public partial class RegisterPlayerFunction : RegisterPlayerFunctionBase { }

    [Function("registerPlayer", "bool")]
    public class RegisterPlayerFunctionBase : FunctionMessage
    {

    }
    public partial class GetPlayerCollectibleCountFunction : GetPlayerCollectibleCountFunctionBase { }

    [Function("getPlayerCollectibleCount", "uint256")]
    public class GetPlayerCollectibleCountFunctionBase : FunctionMessage
    {

    }
    public partial class GetCollectibelDetailsOutputDTO : GetCollectibelDetailsOutputDTOBase { }

    [FunctionOutput]
    public class GetCollectibelDetailsOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "value", 1)]
        public virtual BigInteger Value { get; set; }
        [Parameter("bytes32", "description", 2)]
        public virtual byte[] Description { get; set; }
        [Parameter("bytes32", "name", 3)]
        public virtual byte[] Name { get; set; }
        [Parameter("address", "tokenowner", 4)]
        public virtual string Tokenowner { get; set; }
        [Parameter("uint256", "level", 5)]
        public virtual BigInteger Level { get; set; }
        [Parameter("bytes32", "date", 6)]
        public virtual byte[] Date { get; set; }
        [Parameter("bytes32", "thash", 7)]
        public virtual byte[] Thash { get; set; }
        [Parameter("uint256", "indexMain", 8)]
        public virtual BigInteger IndexMain { get; set; }
    }

    public partial class GetAllCollectibleKeysOutputDTO : GetAllCollectibleKeysOutputDTOBase { }

    [FunctionOutput]
    public class GetAllCollectibleKeysOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256[]", "keys", 1)]
        public virtual List<int> Keys { get; set; }
    }

}
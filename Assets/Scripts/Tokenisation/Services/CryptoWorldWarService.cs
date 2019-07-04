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
        return CryptoWorldWarContract.GetFunction("reigsterPlayer").CreateCallInput();
    }
    public Function GetRegisterUserFunction()
    {
        return CryptoWorldWarContract.GetFunction("reigsterPlayer");
    }

    public CallInput GetAddTokenFunctionInput(int value, string description)
    {
        return CryptoWorldWarContract.GetFunction("addToken").CreateCallInput(new object[] {
   value,
   description
  });
    }
    public CallInput GetAddTokenFunctionInput()
    {
        return CryptoWorldWarContract.GetFunction("addToken").CreateCallInput();
    }
    public Function GetAddTokenFunction()
    {
        return CryptoWorldWarContract.GetFunction("addToken");
    }
    public CallInput GetAllCollectiblesCountFunctionInput()
    {
        return CryptoWorldWarContract.GetFunction("getAllCollectibleCount").CreateCallInput();
    }
    public Function GetAllCollectiblesCountFunction()
    {
        return CryptoWorldWarContract.GetFunction("getAllCollectibleCount");
    }
    public CallInput GetGetCollectibleFunctionInput(int id)
    {
        return CryptoWorldWarContract.GetFunction("getCollectible").CreateCallInput(new object[] {
   id
  });
    }
    public Function GetGetCollectibleFunction()
    {
        return CryptoWorldWarContract.GetFunction("getCollectible");
    }
    public CallInput TransferCollectibleFunction(string to, int id)
    {
        return CryptoWorldWarContract.GetFunction("transferCollectible").CreateCallInput(new object[] {
   id,
   to
  });
    }
    public Function TransferCollectibleFunction()
    {
        return CryptoWorldWarContract.GetFunction("transferCollectible");
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
        var deploymentContract = new CryptoWorldWarDeployment
        {
            Symbol = "CRW",
            Name = "CryptoWorldWar"
        };
        yield
        return TransactionSignedUnityRequest.SignAndSendDeploymentContractTransaction(deploymentContract);
        if (TransactionSignedUnityRequest.Exception != null)
        {
            Debug.Log("Exception from deployment: " + TransactionSignedUnityRequest.Exception.Message);
            yield
            break;
        }
        var hash = TransactionSignedUnityRequest.Result;
        Debug.Log("Deoplyment contract hash: " + hash);
        //Create a unity call request (we have a request for each type of rpc operation)
        var regFunc = new ReigsterPlayerFunction();
        regFunc.FromAddress = Variables.FromAddress;
        regFunc.Gas = 8000000;
        //Call request sends and yield for response	
        yield
        return TransactionSignedUnityRequest.SignAndSendTransaction(regFunc, Variables.ContractAddress);
        yield return TransactionReceiptPollingRequest.PollForReceipt(hash, 2);
        //Each request has a exception and a result. The exception is set when an error occurs.
        //Follows a similar patter to the www and unitywebrequest
        if (TransactionSignedUnityRequest.Exception == null)
        {
            var registered = DecodeRegistrationOutput(TransactionSignedUnityRequest.Result);
            Debug.Log("Results from registration: " + registered);
        }
        else
        {
            Debug.Log(TransactionSignedUnityRequest.Exception.ToString());
        }
    }
    public partial class AddTokenFunction : AddTokenFunctionBase { }

    [Function("addToken", "bool")]
    public class AddTokenFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "value", 1)]
        public virtual Int64 Value
        {
            get;
            set;
        }
        [Parameter("string", "description", 2)]
        public virtual string Description
        {
            get;
            set;
        }
    }
    public partial class GetAllCollectibleCountFunction : GetAllCollectibleCountFunctionBase { }

    [Function("getAllCollectibleCount", "uint256")]
    public class GetAllCollectibleCountFunctionBase : FunctionMessage
    {

    }
    public partial class ReigsterPlayerFunction : ReigsterPlayerFunctionBase { }

    [Function("reigsterPlayer", "bool")]
    public class ReigsterPlayerFunctionBase : FunctionMessage
    {


    }
    public partial class CryptoWorldWarDeployment : CryptoWorldWarDeploymentBase
    {
        public CryptoWorldWarDeployment() : base(BYTECODE) { }
        public CryptoWorldWarDeployment(string byteCode) : base(byteCode) { }
    }

    public class CryptoWorldWarDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "0x604c602c600b82828239805160001a60731460008114601c57601e565bfe5b5030600052607381538281f3fe73000000000000000000000000000000000000000030146080604052600080fdfea165627a7a72305820881dfcbaba85c3efbd41465684c92b2c87dfee245af28f650e453e2d4ec90bf80029";
        public CryptoWorldWarDeploymentBase() : base(BYTECODE) { }
        public CryptoWorldWarDeploymentBase(string byteCode) : base(byteCode) { }
        [Parameter("string", "name", 1)]
        public virtual string Name
        {
            get;
            set;
        }
        [Parameter("string", "symbol", 2)]
        public virtual string Symbol
        {
            get;
            set;
        }
    }
}
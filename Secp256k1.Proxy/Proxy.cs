﻿using System;
using System.IO;
using System.Reflection;

namespace Secp256k1
{
    public static class Proxy
    {
        static Type signaturesType;
        static Type SignaturesType
        {
            get
            {
                if (signaturesType == null)
                {
                    var assemblyName = "Secp256k1." + (IntPtr.Size == 4 ? "x86" : "x64") + ".dll";
                    var assemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyName);
                    Assembly assembly = Assembly.LoadFrom(assemblyPath);
                    var ver = assembly.GetName().Version;
                    if (ver.Major < 1)
                        return null;
                    if (ver.Major == 1 && ver.Minor < 1)
                        return null;
                    signaturesType = assembly.GetType("Secp256k1.Signatures");
                }
                return signaturesType;
            }
        }

        public delegate bool VerifyPrivateKeyDelegate(byte[] privateKey);
        public static VerifyPrivateKeyDelegate VerifyPrivateKey = (VerifyPrivateKeyDelegate)Delegate.CreateDelegate(typeof(VerifyPrivateKeyDelegate), SignaturesType.GetRuntimeMethod("VerifyPrivateKey", new Type[] { typeof(byte[]) }));

        public delegate bool VerifyDelegate(byte[] message, byte[] signature, byte[] publicKey, bool normalizeSignatureOnFailure);
        public static VerifyDelegate Verify = (VerifyDelegate)Delegate.CreateDelegate(typeof(VerifyDelegate), SignaturesType.GetRuntimeMethod("Verify", new Type[] { typeof(byte[]), typeof(byte[]), typeof(byte[]), typeof(bool) }));

        public delegate byte[] SignDelegate(byte[] message, byte[] privateKey);
        public static SignDelegate Sign = (SignDelegate)Delegate.CreateDelegate(typeof(SignDelegate), SignaturesType.GetRuntimeMethod("Sign", new Type[] { typeof(byte[]), typeof(byte[]) }));

        public delegate byte[] SignCompactDelegate(byte[] message, byte[] privateKey, out int recoveryId);
        public static SignCompactDelegate SignCompact = (SignCompactDelegate)Delegate.CreateDelegate(typeof(SignCompactDelegate), SignaturesType.GetRuntimeMethod("SignCompact", new Type[] { typeof(byte[]), typeof(byte[]), typeof(int).MakeByRefType() }));

        public delegate byte[] RecoverKeyFromCompactDelegate(byte[] message, byte[] signature, int recoveryId, bool compressed);
        public static RecoverKeyFromCompactDelegate RecoverKeyFromCompact = (RecoverKeyFromCompactDelegate)Delegate.CreateDelegate(typeof(RecoverKeyFromCompactDelegate), SignaturesType.GetRuntimeMethod("RecoverKeyFromCompact", new Type[] { typeof(byte[]), typeof(byte[]), typeof(int), typeof(bool) }));

        public delegate byte[] GetPublicKeyDelegate(byte[] privateKey, bool compressed);
        public static GetPublicKeyDelegate GetPublicKey = (GetPublicKeyDelegate)Delegate.CreateDelegate(typeof(GetPublicKeyDelegate), SignaturesType.GetRuntimeMethod("GetPublicKey", new Type[] { typeof(byte[]), typeof(bool) }));

        public delegate byte[] NormalizeSignatureDelegate(byte[] signature, out bool wasAlreadyNormalized);
        public static NormalizeSignatureDelegate NormalizeSignature = (NormalizeSignatureDelegate)Delegate.CreateDelegate(typeof(NormalizeSignatureDelegate), SignaturesType.GetRuntimeMethod("NormalizeSignature", new Type[] { typeof(byte[]), typeof(bool).MakeByRefType() }));

        public delegate byte[] SignatureParseDerLaxDelegate(byte[] signature, out bool valid);
        public static SignatureParseDerLaxDelegate SignatureParseDerLax = (SignatureParseDerLaxDelegate)Delegate.CreateDelegate(typeof(SignatureParseDerLaxDelegate), SignaturesType.GetRuntimeMethod("SignatureParseDerLax", new Type[] { typeof(byte[]), typeof(bool).MakeByRefType() }));
    }
}

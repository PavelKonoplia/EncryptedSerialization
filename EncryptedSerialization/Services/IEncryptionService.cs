﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptedSerialization
{
    public interface IEncryptionService
    {
        object Decrypt(byte[] cipherText);
        byte[] Encrypt(object input);
    }
}

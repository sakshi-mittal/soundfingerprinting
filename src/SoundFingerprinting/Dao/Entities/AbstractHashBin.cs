﻿namespace SoundFingerprinting.Dao.Entities
{
    public abstract class AbstractHashBin
    {
        protected AbstractHashBin()
        {
            // no op
        }

        protected AbstractHashBin(long hashBin, int hashTable)
        {
            HashBin = hashBin;
            HashTable = hashTable;
        }

        public int Id { get; set; }

        public long HashBin { get; set; }

        public int HashTable { get; set; }
    }
}
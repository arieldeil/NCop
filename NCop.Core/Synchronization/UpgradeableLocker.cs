﻿using System.Threading;

namespace NCop.Core
{
    public class UpgradeableLocker : AbstractLocker, IUpgradeableLocker
    {
        public UpgradeableLocker(ReaderWriterLockSlim locker)
            : base(locker) {
            if (Locker.IsWriteLockHeld) {
                Locker.ExitWriteLock();
                return;
            }

            while (!LockAcquired) {
                LockAcquired = Locker.TryEnterUpgradeableReadLock(0);
            }
        }

        public IDowngradeableLocker UpgradeToWriterLock() {
            return new DowngradeableLocker(Locker);
        }
    }
}
﻿using System;
using System.Threading;

namespace ReaderWriterLockSlim_Type
{
    class Program
    {
        private bool condition;
        private ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

        public void Method1()
        {
            readerWriterLockSlim.EnterReadLock(); // Noncompliant
            if (condition)
            {
                readerWriterLockSlim.ExitReadLock();
            }
        }

        public void Method2()
        {
            readerWriterLockSlim.EnterWriteLock(); // Noncompliant
            if (condition)
            {
                readerWriterLockSlim.ExitWriteLock();
            }
        }

        public void Method3()
        {
            readerWriterLockSlim.EnterUpgradeableReadLock(); // Noncompliant
            if (condition)
            {
                readerWriterLockSlim.ExitUpgradeableReadLock();
            }
        }

        public void Method4()
        {
            if (readerWriterLockSlim.TryEnterReadLock(42)) // Noncompliant
            {
            }
            else
            {
                readerWriterLockSlim.ExitReadLock();
            }
        }

        public void Method5()
        {
            if (readerWriterLockSlim.TryEnterReadLock(new TimeSpan(42))) // Noncompliant
            {
            }
            else
            {
                readerWriterLockSlim.ExitReadLock();
            }
        }

        public void Method6()
        {
            if (readerWriterLockSlim.TryEnterWriteLock(42)) // Noncompliant
            {
            }
            else
            {
                readerWriterLockSlim.ExitWriteLock();
            }
        }

        public void Method7()
        {
            if (readerWriterLockSlim.TryEnterWriteLock(new TimeSpan(42))) // Noncompliant
            {
            }
            else
            {
                readerWriterLockSlim.ExitWriteLock();
            }
        }

        public void Method8()
        {
            if (readerWriterLockSlim.TryEnterUpgradeableReadLock(42)) // Noncompliant
            {
            }
            else
            {
                readerWriterLockSlim.ExitReadLock();
            }
        }

        public void Method9()
        {
            if (readerWriterLockSlim.TryEnterUpgradeableReadLock(new TimeSpan(42))) // Noncompliant
            {
            }
            else
            {
                readerWriterLockSlim.ExitReadLock();
            }
        }


        public void Method10()
        {
            try
            {
                readerWriterLockSlim.EnterUpgradeableReadLock();
                readerWriterLockSlim.EnterWriteLock(); // Compliant
                if (condition)
                {
                    readerWriterLockSlim.ExitWriteLock();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                readerWriterLockSlim.ExitUpgradeableReadLock();
            }
        }

        public void Method11(string arg)
        {
            try
            {
                readerWriterLockSlim.EnterUpgradeableReadLock();
                readerWriterLockSlim.EnterWriteLock();
                try
                {
                    Console.WriteLine(arg.Length);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    readerWriterLockSlim.ExitWriteLock();
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                readerWriterLockSlim.ExitUpgradeableReadLock();
            }
        }

        public void Method12()
        {
            readerWriterLockSlim.EnterReadLock(); // Compliant
            readerWriterLockSlim.ExitReadLock();
        }

        public void Method13()
        {
            readerWriterLockSlim.EnterReadLock(); // Compliant, this rule doesn't care if it was released with correct API
            readerWriterLockSlim.ExitWriteLock();
        }

        public void WrongOrder()
        {
            readerWriterLockSlim.ExitReadLock();
            readerWriterLockSlim.EnterReadLock(); // Noncompliant

            var a = new ReaderWriterLockSlim();
            a.ExitWriteLock();
            a.EnterWriteLock(); // Noncompliant

            var b = new ReaderWriterLockSlim();
            b.ExitUpgradeableReadLock();
            b.TryEnterReadLock(1); // Noncompliant

            var c = new ReaderWriterLockSlim();
            c.ExitReadLock();
            c.TryEnterWriteLock(1); // Noncompliant

            var d = new ReaderWriterLockSlim();
            d.ExitReadLock();
            d.EnterUpgradeableReadLock(); // Noncompliant

            var e = new ReaderWriterLockSlim();
            e.ExitReadLock();
            e.TryEnterUpgradeableReadLock(1); // Noncompliant
        }

        public void Method14()
        {
            readerWriterLockSlim.EnterReadLock(); // Noncompliant, this rule doesn't care if it was released with correct API
            if (condition)
            {
                readerWriterLockSlim.ExitWriteLock();
            }
        }

        public void Method15()
        {
            if (readerWriterLockSlim.TryEnterReadLock(42)) // Noncompliant FP
            {
                readerWriterLockSlim.ExitReadLock();
            }
        }

        public void EarlyExit()
        {
            if (readerWriterLockSlim.TryEnterReadLock(0) == false) // Noncompliant FP - https://github.com/SonarSource/sonar-dotnet/issues/5415
            {
                return;
            }

            try
            {
            }
            finally
            {
                readerWriterLockSlim.ExitReadLock();
            }
        }

        public void EarlyExit_UnaryOperator()
        {
            if (!readerWriterLockSlim.TryEnterReadLock(0)) // Noncompliant FP - https://github.com/SonarSource/sonar-dotnet/issues/5415
            {
                return;
            }

            try
            {
            }
            finally
            {
                readerWriterLockSlim.ExitReadLock();
            }
        }

        public void Throw_Finally(object param)
        {
            readerWriterLockSlim.EnterReadLock(); // Noncompliant FP, there are multiple ocurrences on peach. Should be handled by throw implementation.
            try
            {
                if (param == null)
                    throw new ObjectDisposedException("");

                return;
            }
            finally
            {
                readerWriterLockSlim.ExitReadLock();
            }
        }

        public void IsReadLockHeld()
        {
            readerWriterLockSlim.EnterReadLock(); // Noncompliant FP, https://github.com/SonarSource/sonar-dotnet/issues/5416
            if (readerWriterLockSlim.IsReadLockHeld)
            {
                readerWriterLockSlim.ExitReadLock();
            }
        }

        public void IsWriteLockHeld()
        {
            readerWriterLockSlim.EnterWriteLock(); // Noncompliant FP, https://github.com/SonarSource/sonar-dotnet/issues/5416
            if (readerWriterLockSlim.IsWriteLockHeld)
            {
                readerWriterLockSlim.ExitWriteLock();
            }
        }
    }
}

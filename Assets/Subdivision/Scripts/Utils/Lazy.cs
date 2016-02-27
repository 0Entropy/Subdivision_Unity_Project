using UnityEngine;
using System.Collections;
using System;

public class Lazy<T> where T : new()
{

    private T value;
    public T Value {
        get
        {
            if (!isValeCreated)
            {
                if(mValueFactory != null)
                {
                    value = mValueFactory();
                }
                else
                {
                    value = new T();
                }
                isValeCreated = true;
            }

            return value;
        }

    }

    private bool isValeCreated;
    public bool IsValueCreated {
        get
        {
            return isValeCreated;
        }
    }

    private Func<T> mValueFactory;

    public Lazy()
    {
        isValeCreated = false;
    }

    public Lazy(Func<T> valueFactory)
    {
        isValeCreated = false;
        mValueFactory = valueFactory;
    }
    //
    // Summary:
    //     Initializes a new instance of the System.Lazy`1 class. When lazy initialization
    //     occurs, the default constructor of the target type is used.
    //public Lazy();
    //
    // Summary:
    //     Initializes a new instance of the System.Lazy`1 class that uses the default constructor
    //     of T and the specified thread-safety mode.
    //
    // Parameters:
    //   mode:
    //     One of the enumeration values that specifies the thread safety mode.
    //
    // Exceptions:
    //   T:System.ArgumentOutOfRangeException:
    //     mode contains an invalid value.
    //public Lazy(LazyThreadSafetyMode mode);
    //
    // Summary:
    //     Initializes a new instance of the System.Lazy`1 class. When lazy initialization
    //     occurs, the default constructor of the target type and the specified initialization
    //     mode are used.
    //
    // Parameters:
    //   isThreadSafe:
    //     true to make this instance usable concurrently by multiple threads; false to
    //     make the instance usable by only one thread at a time.
    //public Lazy(bool isThreadSafe);
    //
    // Summary:
    //     Initializes a new instance of the System.Lazy`1 class. When lazy initialization
    //     occurs, the specified initialization function is used.
    //
    // Parameters:
    //   valueFactory:
    //     The delegate that is invoked to produce the lazily initialized value when it
    //     is needed.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     valueFactory is null.
    //public Lazy(Func<T> valueFactory);
    //
    // Summary:
    //     Initializes a new instance of the System.Lazy`1 class that uses the specified
    //     initialization function and thread-safety mode.
    //
    // Parameters:
    //   valueFactory:
    //     The delegate that is invoked to produce the lazily initialized value when it
    //     is needed.
    //
    //   mode:
    //     One of the enumeration values that specifies the thread safety mode.
    //
    // Exceptions:
    //   T:System.ArgumentOutOfRangeException:
    //     mode contains an invalid value.
    //
    //   T:System.ArgumentNullException:
    //     valueFactory is null.
    //public Lazy(Func<T> valueFactory, LazyThreadSafetyMode mode);
    //
    // Summary:
    //     Initializes a new instance of the System.Lazy`1 class. When lazy initialization
    //     occurs, the specified initialization function and initialization mode are used.
    //
    // Parameters:
    //   valueFactory:
    //     The delegate that is invoked to produce the lazily initialized value when it
    //     is needed.
    //
    //   isThreadSafe:
    //     true to make this instance usable concurrently by multiple threads; false to
    //     make this instance usable by only one thread at a time.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     valueFactory is null.
    //public Lazy(Func<T> valueFactory, bool isThreadSafe);

    //
    // Summary:
    //     Gets a value that indicates whether a value has been created for this System.Lazy`1
    //     instance.
    //
    // Returns:
    //     true if a value has been created for this System.Lazy`1 instance; otherwise,
    //     false.
    //public bool IsValueCreated { get; }
    //
    // Summary:
    //     Gets the lazily initialized value of the current System.Lazy`1 instance.
    //
    // Returns:
    //     The lazily initialized value of the current System.Lazy`1 instance.
    //
    // Exceptions:
    //   T:System.MemberAccessException:
    //     The System.Lazy`1 instance is initialized to use the default constructor of the
    //     type that is being lazily initialized, and permissions to access the constructor
    //     are missing.
    //
    //   T:System.MissingMemberException:
    //     The System.Lazy`1 instance is initialized to use the default constructor of the
    //     type that is being lazily initialized, and that type does not have a public,
    //     parameterless constructor.
    //
    //   T:System.InvalidOperationException:
    //     The initialization function tries to access System.Lazy`1.Value on this instance.
    //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
    //public T Value { get; }

    //
    // Summary:
    //     Creates and returns a string representation of the System.Lazy`1.Value property
    //     for this instance.
    //
    // Returns:
    //     The result of calling the System.Object.ToString method on the System.Lazy`1.Value
    //     property for this instance, if the value has been created (that is, if the System.Lazy`1.IsValueCreated
    //     property returns true). Otherwise, a string indicating that the value has not
    //     been created.
    //
    // Exceptions:
    //   T:System.NullReferenceException:
    //     The System.Lazy`1.Value property is null.
    //public override string ToString();
}

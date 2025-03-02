using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWithSetUp  
{
    public bool IsInitialized { get; }
    
    public void SetUp();
    
    public void TearDown();
}

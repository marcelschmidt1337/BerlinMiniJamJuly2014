using System;
using UEDS;
using IOBridge;
using System.IO;
using System.Xml.Serialization;

#if UNITY_METRO
namespace AssemblyCSharp
{
	
	[UEDS.EditorSettingIOProvider]
	public class AsyncIOProvider : IGlobalEditorSettingsIOProvider
	{
		public AsyncIOProvider ()
		{
		}
		
		SerializedRoot mLoadedData;
		
		WrappedIO mLoadProcess;
		WrappedIO mSaveProcess;
		WrappedIO mExistsProcess;
		
		#region IGlobalEditorSettingsIOProvider implementation
		public void Save<T> (T pObject, string pPath) where T : SerializedRoot, new ()
		{
			StringWriter sw = new StringWriter();
			
			XmlSerializer s = new XmlSerializer(typeof(T));
			s.Serialize(sw, pObject);
			
			mSaveProcess = Storage.Save(pPath, sw.ToString());
			
		}

		public void Load<T> (string pPath) where T : SerializedRoot, new ()
		{
			mLoadedData = null;
			mLoadProcess = Storage.Load(pPath);
		}

		public System.Collections.IEnumerator Exists (ExistCheckParams pParams)
		{
			mExistsProcess = Storage.Exists( pParams.mPath );
			
			while(!mExistsProcess.IsDone)
				yield return 0;
			
			pParams.mResult = mExistsProcess.FileExists;
			
		}

		public void CreateEmpty<T> () where T : SerializedRoot,new ()
		{
			mLoadedData = new T();
		}

		public T GetData<T> () where T : SerializedRoot, new ()
		{
			if(mLoadedData == null && mLoadProcess != null)
			{
				TextReader tr = new StringReader( mLoadProcess.GetContent() );
				
				XmlSerializer s = new XmlSerializer(typeof(T));
				mLoadedData = s.Deserialize(tr) as T;
				return mLoadedData as T;
			}else
			{
				return null;
			}
			
		}

		public bool WriterFinished {
			get {
				
				if(mSaveProcess == null)
					return true;
				return mSaveProcess.IsDone;
			}
		}

		public bool WriterHasError {
			get {
				return false;
			}
		}

		public string WriterError {
			get {
				return "";
			}
		}

		public bool LoaderFinished {
			get {
				if(mLoadProcess == null)
					return false;
				
				return mLoadProcess.IsDone;
			}
		}

		public bool LoaderHasError {
			get {
				if(mLoadProcess == null)
					return false;
				
				try
				{
					mLoadProcess.GetContent();
				}catch
				{
					return true;
				}
				return false;
			}
		}

		public string LoaderError {
			get {
				try
				{
					mLoadProcess.GetContent();
				}catch(Exception e)
				{
					return e.Message;
				}
				return "";
			}
		}
		#endregion
	}
}
#endif

# QSingleton
rm ./Client/Assets/QFramework/Core/Singleton
ln -s ../../../../Submodule/QSingleton/Client/Assets/QSingleton ./Client/Assets/QFramework/Core
mv ./Client/Assets/QFramework/Core/QSingleton ./Client/Assets/QFramework/Core/Singleton
echo "Setup finish !"
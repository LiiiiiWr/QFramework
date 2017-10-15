# QSingleton
rm ./Client/Assets/QFramework/Core/Singleton
ln -s ../../../../Submodule/QSingleton/Client/Assets/QSingleton ./Client/Assets/QFramework/Core
mv ./Client/Assets/QFramework/Core/QSingleton ./Client/Assets/QFramework/Core/Singleton

# QChain
rm ./Client/Assets/QFramework/Core/Unity/Extension
ln -s ../../../../../Submodule/QChain/Client/Assets/QChain/Unity/Extension ./Client/Assets/QFramework/Core/Unity/Extension
echo "Setup finish !"
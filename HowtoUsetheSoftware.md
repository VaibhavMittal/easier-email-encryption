# <font color='darkblue' face='book antiqua'> How To's and Videos </font> #

---



---


# Video Tutorials #
<a href='Hidden comment: 
== <u>Outlook Add-in</u> ==
* <wiki:video url="http://www.youtube.com/watch?v=3LkNlTNHZzE"/>
* [http://www.youtube.com Add-in Installation, Sending and Receiving encrypted messages.]
<font color="White">=== <u>Standalone Application

Unknown end tag for &lt;/u&gt;

===
* [http://www.youtube.com Reading encrypted messages through a standalone windows application.]


Unknown end tag for &lt;/font&gt;


'></a>

Coming Soon...

---


# Screenshot Tutorials #
## <u>Outlook Add-in</u> ##
> With ECube you can secure your email messages, including attachments, by just using a password.

### ECube Installation ###

**Prerequisites**

  * Outlook 2007 or 2010

  * .NET Framework 4.0 or higher.

  * Microsoft Visual Studio 2010 Tools for Office Runtime (x86 and x64)

NOTE: All the prerequisites except Outlook software will be downloaded and installed automatically by the add-in setup.

**Operating System**

Windows

**NOTE**: The ECube v1.0 outlook add-in (prototype) has been successfully tested on Outlook 2007 and 2010 with Windows 7 and Windows 8.



** **

<font size='4'><b>Installation Steps</b></font>

1)      Download the “ECube installer.zip”.

![http://easier-email-encryption.googlecode.com/files/Installation_Download_Installer.jpg](http://easier-email-encryption.googlecode.com/files/Installation_Download_Installer.jpg)

2)      Extract the contents of the ZIP file and Run “Setup.exe”

![http://easier-email-encryption.googlecode.com/files/Installation_1.jpg](http://easier-email-encryption.googlecode.com/files/Installation_1.jpg)

3)      Program will check for the necessary prerequisites, if something is missing it will ask you to download and install it. Just click the Accept button and it will automatically download and install the necessary prerequisites.

http://easier-email-encryption.googlecode.com/files/Installation_Prerequisites_Install.JPG

4)      After all the prerequisites have been installed the ECube installation will start automatically.

5)      Just click Next button and continue with the installation.

![http://easier-email-encryption.googlecode.com/files/Installation_2.jpg](http://easier-email-encryption.googlecode.com/files/Installation_2.jpg)

6)      Open the Microsoft Outlook application. It will ask for the permissions to allow the add-in or not just click on the “Run” button and it will include the add-in (just once).

![http://easier-email-encryption.googlecode.com/files/Installation_3.jpg](http://easier-email-encryption.googlecode.com/files/Installation_3.jpg)

7)      Installation finished the add-in should be ready to use now.


---




** **
### Sending an Encrypted Message ###

1)      Click on the “New Mail Message” button to open a new message composition window.

![http://easier-email-encryption.googlecode.com/files/1_Encrypt_Email_Tab.jpg](http://easier-email-encryption.googlecode.com/files/1_Encrypt_Email_Tab.jpg)

2)      Compose the message completely (including the Recipient(s) email address) and attach any file(s) if required.

![http://easier-email-encryption.googlecode.com/files/2-Complete_Email_Message.jpg](http://easier-email-encryption.googlecode.com/files/2-Complete_Email_Message.jpg)

3)      After composing the message, go to “Encrypt Email” tab on the ribbon and click the “Quick Security” button.

![http://easier-email-encryption.googlecode.com/files/3-Quick_Security_Button.jpg](http://easier-email-encryption.googlecode.com/files/3-Quick_Security_Button.jpg)

4)      a) Enter a password, preferably a strong one (at least 10 characters containing alphabets, numbers and special characters). Renter the password to confirm.

b) Give a Password Hint (optional) here which might assist the receiver in guessing the password example: “the first movie we watched in Brisbane”.

c) Click the Send Encrypted Message button to encrypt and send your email message

![http://easier-email-encryption.googlecode.com/files/4-Quick_Security_DialogBox.jpg](http://easier-email-encryption.googlecode.com/files/4-Quick_Security_DialogBox.jpg)

5)      After the message has been encrypted and is being sent (in the background), a choice panel will allow you to either:

  * Save the Encrypted message in Sent folder. (Default)

  * Save the Decrypted message in Sent folder.

  * Delete the message permanently (to remove complete trace of the message).

Select an option which seems most appropriate to you and click “Done”.

![http://easier-email-encryption.googlecode.com/files/5-User_Choice_Options.jpg](http://easier-email-encryption.googlecode.com/files/5-User_Choice_Options.jpg)

---




** **
### Reading an Encrypted Message ###

1)      When you receive an email encrypted by ECube then it will have some “Instructions” as the message body and the real message body and the attachment(s) will be encrypted.

![http://easier-email-encryption.googlecode.com/files/6-Decrypting_the_Message.jpg](http://easier-email-encryption.googlecode.com/files/6-Decrypting_the_Message.jpg)

2)      Open the encrypted message by double clicking on it (or press Enter). The “Decryption dialog box” will appear along with the message window. Read the _Hint_ and enter the _Password_ in the box. Press Enter or click the “Display Message” button to decrypt and display the original message.

![http://easier-email-encryption.googlecode.com/files/7-Decryption_PasswordDialogBox.jpg](http://easier-email-encryption.googlecode.com/files/7-Decryption_PasswordDialogBox.jpg)


3)      You can also reopen the Decryption dialog box by going in to the Decrypt Email tab and clicking the “Decrypt Email” button.

![http://easier-email-encryption.googlecode.com/files/8-Decryption_Ribbon.jpg](http://easier-email-encryption.googlecode.com/files/8-Decryption_Ribbon.jpg)

---


### Un-Installing ECube ###

1)      Go to Start&gt;Control Panel” (click Start and type “Control Panel”).

2)      Open Programs and Features.

3)      Select “ECube-V1.0” and click Uninstall.

4)      Follow the un-installation steps; the add-in should be removed.

---


### Enabling and Disabling Add-in(s) in Outlook 2007, 2010 ###

1)      In Outlook application go to Tools&gt;Trust Centre

2)      Navigate to “Add-ins” menu

3)      At the bottom of the window there will be a “Manage” area, select “COM Add-ins” from the combo box and click the _“Go...”_ button.

![http://easier-email-encryption.googlecode.com/files/Enabling_Disabling_addins_1.jpg](http://easier-email-encryption.googlecode.com/files/Enabling_Disabling_addins_1.jpg)

4)      Check or uncheck the add-in you want to enable or disable and click Ok.

![http://easier-email-encryption.googlecode.com/files/Enabling_Disabling_addins_2.jpg](http://easier-email-encryption.googlecode.com/files/Enabling_Disabling_addins_2.jpg)

## Standalone Message Decryptor ##
1)      Download and extract the [ECube Standalone Decryptor v1.0.zip](http://easier-email-encryption.googlecode.com/files/EcubeStandaloneDecryptor-v1.0Executable.zip).

2)      Download and save the "smime.p7m" file from a web email client or any other.

3)      Run the **"ECube Message Decryptor"** application.

4)      Browse for the smime.p7m file path and enter the password. Click "Decrypt Message" to display the message in a web page.

---
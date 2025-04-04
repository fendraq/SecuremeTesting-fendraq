import React, {useEffect, useState, useRef} from "react";
import { Box, TextField, IconButton, Paper, Typography, Avatar, Stack, Container } from "@mui/material";
import { styled } from "@mui/system";
import SendIcon from '@mui/icons-material/Send';
import Button from "@mui/material/Button";

export default function Chat( {customerChat, userCases, selectedCase, chatToken} ){
  
  // definierar props
  const [isCustomerChat, setIsCustomerChat] = useState(customerChat);
  const [fetchedUserCases, setFetchedUserCases] = useState(userCases || []);
  const [fetchedCaseId, setFetchedCaseId] = useState(selectedCase?.id || null);

  // uppdatera state om prop ändras
  useEffect(() => {
    setIsCustomerChat(customerChat);
    setFetchedUserCases(userCases || []);
    setFetchedCaseId(selectedCase?.id || null);
  }, [customerChat, userCases, selectedCase]);
  
  // definierar chatData 
  const [chatData, setChatData] = useState({ 
    caseDetails: {},
    messages: [],
    user: {},
  });
  
  // definierar newMessage för att hantera nytt meddelande
  const [newMessage, setNewMessage] = useState(() => ({
    case_id: null,
    is_sender_customer: isCustomerChat,
  }));
  // använda ref för input av nytt meddelande. Motverkar rerendering
  const inputRef =useRef();
  
  //Deklarerar fetch data till kundens chatt:
  const fetchCustomerChatData = (chatToken) => { 
    fetch(`/api/chat/case/${chatToken}`)
      .then(response => response.json())
      .then(data => {
        setChatData(data);
        
        setNewMessage(prev => ({
          ...prev,
          case_id: data.caseDetails?.id || null,
        }));
      })
      .catch(error => console.log("fetch chatCustomerData error:", error));
  }
  
  //Deklarerar fetch data till medarbetarens chatt:
  const fetchUserChatData = (id) => {
    if (!userCases || userCases.length === 0){
      console.log("No user cases available");
      return;
    }
    // väljer ut data baserad på det klickade caset
    const selectedCase = fetchedUserCases.find(caseItem => caseItem.id === fetchedCaseId);
    
    fetch(`/api/chat/backoffice/${fetchedCaseId}`)
      .then(response => response.json())
      .then(data => {
        const updatedData = {
          ...data,
          caseDetails: {
            ...data.caseDetails,
            ...selectedCase,
          }
        };
        
        setChatData(updatedData);

        setNewMessage(prev => ({
          ...prev,
          case_id: fetchedCaseId || null,
        }));
      })
      .catch(error => console.log("fetch chatUserChatData error:", error));
  }
  // Använder useEffect tillsammans med if-statement kund/medarbetare
  useEffect(() => {
    //Statement som hanterar vilken data som ska hämtas beroende på kund/medarbetare
    if (isCustomerChat) {
      fetchCustomerChatData(chatToken);
    } else {
      fetchUserChatData();
    }
  }, [isCustomerChat, userCases, chatToken]);
  
  // hantera stäng ärende för medarbetare
  const handleCloseCase = async (id) => {
    if (!id) {
      console.error("No provided id for closing case.");
      return;
    }
    try {
      const response = await fetch(`/api/chat/close-case/${id}`, {
        method: "PATCH",
        headers: {"Content-Type": "application/json"},
        body: JSON.stringify({ id })
      });
      
      if (!response.ok){
        console.log(`Failed to close case with id ${id}. Status is ${response.status}`);
      }
      //Rerender
      
    }
    catch(error){
      console.error("Error when closing case: ", error);
    }
  };
  // hantera skicka nytt ärende till db
  const handleNewMessage = async () => {
    const newMessageText = inputRef.current.value.trim();
    
    if (!newMessage.case_id) {
      alert("Make sure that there is a case connected before sending");
      return;
    }
    
    if (!newMessageText) {
      alert("No message entered");
      return;
    }
    
    try {
      const response = await fetch(`/api/chat/new-message`, {
        method: "POST",
        headers: {"Content-Type": "application/json"},
        body: JSON.stringify({
          case_id: newMessage.case_id,
          text: newMessageText,
          is_sender_customer: isCustomerChat,
        }),
      });
      
      if (response.ok) {
        //Rerender här
        inputRef.current.value = "";
      }
      else 
      {
        const errorData = await response.json();
        console.error("Error response:", errorData);
        alert("Error sending message");
      }
    }
    catch (error){
      console.log("Error sending new messagge:", error);
    }
  };
  
  //Style and statements in elements
  const ChatContainer = styled(Paper)(({ theme }) => ({
    height: isCustomerChat ? "70vh" : "40vh",
    display: "flex",
    flexDirection: "column",
    borderRadius: 16,
    overflow: "hidden",
    boxShadow: "0 4px 20px"
  }));
  
  const MessagesContainer = styled(Box)({
    flex: 1,
    overflowY: "auto",
    padding: "20px",
    "&::-webkit-scrollbar": {
      width: "6px",
    },
    "&::-webkit-scrollbar-track": {
      background: "transparent",
    },
    "&::-webkit-scrollbar-thumb": {
      background: "grey",//Color scheme?
      borderRadius: "3px",
    },
  });

  const MessageBubble = styled(Box, { 
    //Kolla igen på hur jag ska hantera detta. Renderar men strul med DOM error.
    // Ev. lösning: import forwardRef from react
    shouldForwardProp: (prop) => prop !== "isUser",
  })(({ isUser }) => ({
    display: "flex",
    alignItems: "flex-start",
    marginBottom: "14px",
    flexDirection: isUser ? "row" : "row-reverse",
  }));
  
  const MessageContent = styled(Paper)(({ isUser }) => ({ 
    padding: "12px 16px",
    borderRadius: "16px",
    maxWidth: "70%",
    marginLeft: isUser ? 0 : "12px",
    marginRight: isUser ? "12px" : 0,
    backgroundColor: isUser ? "#e4c2ff" : "#f5f5f5", 
    color: isUser ? "#000" : "#000", 
  }));
  
  const InputContainer = styled(Box)({
    padding: "20px",
    borderTop: "1px solid rgba(0, 0, 0, 0.1)",
  });
  
  const ChatHeader = styled(Box) ({
    display: "flex",
    flexDirection: "row",
    justifyContent: "space-between",
    paddingRight: "15px",
    paddingLeft: "15px",
  });
  
  const StyledAvatar = styled(Avatar)(({ isUser }) =>({
    /*backgroundColor: isUser ? "#e4c2ff" : "#f5f5f5",*/
  }));
  
  return (
    <>
      <h1>Chatt</h1>
      <Container  maxWidth="md" sx={{ mt: 4 }}>
        <ChatContainer>
          <ChatHeader>
            <h4>Customer: {chatData?.caseDetails?.customer_first_name}</h4>
            <h3>{chatData?.caseDetails?.title}</h3>
            <h4>Case handler: {chatData?.user?.user_name}</h4>
          </ChatHeader>
          <MessagesContainer>
            {chatData?.messages
              ?.sort((a, b) => new Date(a.timestamp) - new Date(b.timestamp))
              .map((message) => ( 
              <MessageBubble key={message.id} isUser={message.is_sender_customer}>
                <StyledAvatar />
                {/*Add avatar details*/}
                <MessageContent isUser={message.is_sender_customer}>
                  <Typography variant="body1" type="div">
                    {message.text}
                  </Typography>
                  <Typography 
                    variant="caption"
                    sx={{ opacity: 0.7, mt: 0.5, display: "block"}}
                  >
                    {message.timestamp}
                  </Typography>
                </MessageContent>
              </MessageBubble>
            ))}
          </MessagesContainer>
          <InputContainer>
            <Stack direction="row" spacing={2}>
              <TextField 
                inputRef={inputRef}
                fullWidth
                multiline
                maxRows={4}
              />
              <IconButton
                onClick={handleNewMessage}
                color="inherit"
                aria-label="Send message..."
                sx={{
                  backgroundColor: "#fff", 
                  color: "black",
                }}
              >
                <SendIcon />
              </IconButton>
              {!customerChat &&(
                <Button
                  variant="outlined"
                  color="inherit"
                  onClick={() => handleCloseCase(chatData?.caseDetails?.id)}
                >
                  Close Case
                </Button>
              )}
            </Stack>
          </InputContainer>
        </ChatContainer>
      </Container>
    
    </>
  )
}
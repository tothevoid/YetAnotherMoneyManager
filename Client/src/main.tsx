import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import "./i18n";
import App from './App.tsx'
import { Box, ChakraProvider } from '@chakra-ui/react'
import "react-datepicker/dist/react-datepicker.css";
import { darkTheme } from './theme.ts';
import { UserProvider } from './contexts/UserProfileContext.tsx';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ChakraProvider value={darkTheme}>
      <UserProvider>
        <Box backgroundColor="background_main">
            <App />
        </Box>
      </UserProvider>
    </ChakraProvider>
  </StrictMode>,
)

import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import "./i18n";
import App from './App.tsx'
import { Box, ChakraProvider } from '@chakra-ui/react'
import "react-datepicker/dist/react-datepicker.css";
import { darkTheme } from './theme.ts';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ChakraProvider value={darkTheme}>
      <Box backgroundColor="background_main">
          <App />
      </Box>
    </ChakraProvider>
  </StrictMode>,
)

import { IconType } from 'react-icons';
import {
    MdLaptop,
    MdSmartphone,
    MdTablet,
    MdDesktopWindows,
    MdOutlineDevices
} from 'react-icons/md';
import { Nullable } from './nullable';

export interface DeviceInfo {
    name: string;
    isMobile: boolean;
    icon: IconType;
}

export const parseDeviceAndBrowser = (userAgent?: Nullable<string>): DeviceInfo => {
    if (!userAgent) {
        return { name: '', isMobile: false, icon: MdOutlineDevices };
    }

    const ua = userAgent.toLowerCase();
    let browser = 'Browser';
    let os = 'OS';
    let icon: IconType = MdDesktopWindows;

    // Detect Browser
    if (ua.includes('edg/')) {
        browser = 'Edge';
    } else if (ua.includes('chrome/') && !ua.includes('chromium/')) {
        browser = 'Chrome';
    } else if (ua.includes('firefox/')) {
        browser = 'Firefox';
    } else if (ua.includes('safari/') && !ua.includes('chrome/')) {
        browser = 'Safari';
    } else if (ua.includes('opera/') || ua.includes('opr/')) {
        browser = 'Opera';
    }

    // Detect OS & Device type
    if (ua.includes('iphone') || ua.includes('ipod')) {
        os = 'iOS';
        icon = MdSmartphone;
    } else if (ua.includes('ipad')) {
        os = 'iPadOS';
        icon = MdTablet;
    } else if (ua.includes('android')) {
        os = 'Android';
        icon = MdSmartphone;
    } else if (ua.includes('windows')) {
        os = 'Windows';
        icon = MdLaptop;
    } else if (ua.includes('macintosh') || ua.includes('mac os')) {
        os = 'macOS';
        icon = MdLaptop;
    } else if (ua.includes('linux')) {
        os = 'Linux';
        icon = MdDesktopWindows;
    }

    return {
        name: `${browser} • ${os}`,
        isMobile: icon === MdSmartphone || icon === MdTablet,
        icon
    };
};

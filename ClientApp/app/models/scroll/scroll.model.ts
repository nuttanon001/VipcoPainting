export interface Scroll {
    Skip?: number;
    Take?: number;
    SortField?: string;
    SortOrder?: number;
    Filter?: string;
    Reload?: boolean;
    Where?: string;
    HasCondition?: boolean;
}